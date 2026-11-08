using System.Numerics;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Station.Events;
using Content.Server.Station.Systems;
using Content.Shared.Atmos;
using Content.Shared.Gravity;
using Content.Shared.Light.Components;
using Content.Shared.Paper;
using Content.Shared.Shuttles.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;

namespace Content.Server.SSTNO;

/// <summary>
/// Turns the SSTNO developer map's placeholder grid into a grass pad.
/// The grid stays the station grid so map-load tests and player spawns agree on where the round is.
/// </summary>
public sealed class SstnoDeveloperTestAreaSystem : EntitySystem
{
    [Dependency] private AtmosphereSystem _atmos = default!;
    [Dependency] private ITileDefinitionManager _tiles = default!;
    [Dependency] private ServerStationSystem _station = default!;
    [Dependency] private SharedMapSystem _map = default!;
    [Dependency] private SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SstnoDeveloperTestAreaComponent, StationPostInitEvent>(OnStationPostInit);
    }

    private void OnStationPostInit(Entity<SstnoDeveloperTestAreaComponent> ent, ref StationPostInitEvent args)
    {
        // Loaded grids are still paused. Entity queries skip paused entities, so use the station grid list.
        var grid = _station.GetLargestGrid(ent.Owner);
        if (grid is not { } gridUid)
        {
            Log.Error("SSTNO developer station has no grid.");
            return;
        }

        if (Transform(gridUid).MapUid is not { } map)
        {
            Log.Error("SSTNO developer grid is not on a map.");
            return;
        }

        if (!TryComp<MapGridComponent>(gridUid, out var mapGrid))
        {
            Log.Error("SSTNO developer grid is missing MapGrid.");
            return;
        }

        PaintGrass(gridUid, mapGrid, ent.Comp);
        GiveAirAndLight(map, gridUid, ent.Comp.MapLightColor);
        AnchorGrid(gridUid);
        SpawnCamp(gridUid);

        if (TryComp<MapComponent>(map, out var mapComp))
            _map.SetPaused(mapComp.MapId, false);
    }

    private void PaintGrass(EntityUid gridUid, MapGridComponent mapGrid, SstnoDeveloperTestAreaComponent comp)
    {
        var definition = _tiles[comp.GrassTile];
        var grass = new Tile(definition.TileId);
        var tiles = new List<(Vector2i GridIndices, Tile Tile)>();
        for (var x = -comp.PadRadius; x <= comp.PadRadius; x++)
        {
            for (var y = -comp.PadRadius; y <= comp.PadRadius; y++)
                tiles.Add((new Vector2i(x, y), grass));
        }

        _map.SetTiles(gridUid, mapGrid, tiles);
    }

    private void GiveAirAndLight(EntityUid map, EntityUid grid, Color lightColor)
    {
        var moles = new float[Atmospherics.AdjustedNumberOfGases];
        moles[(int)Gas.Oxygen] = Atmospherics.OxygenMolesStandard;
        moles[(int)Gas.Nitrogen] = Atmospherics.NitrogenMolesStandard;
        _atmos.SetMapAtmosphere(map, false, new GasMixture(moles, Atmospherics.T20C));

        // Gravity is read from the grid the mob is standing on.
        var gravity = EnsureComp<GravityComponent>(grid);
        gravity.Enabled = true;
        gravity.Inherent = true;
        Dirty(grid, gravity);

        var light = EnsureComp<MapLightComponent>(map);
        light.AmbientLightColor = lightColor;
        Dirty(map, light);
    }

    private void AnchorGrid(EntityUid gridUid)
    {
        // The empty debug grid is a dynamic shuttle. This pad should not fly or be pushed off the spawn.
        RemComp<ShuttleComponent>(gridUid);
        _physics.TrySetBodyType(gridUid, BodyType.Static);
    }

    private void SpawnCamp(EntityUid gridUid)
    {
        EntityCoordinates At(float x, float y) => new(gridUid, new Vector2(x, y));

        SpawnAttachedTo("SpawnPointSSTNOCivilian", At(0.5f, 0.5f));
        SpawnAttachedTo("SpawnPointLatejoin", At(1.5f, 0.5f));
        SpawnAttachedTo("SpawnPointObserver", At(0.5f, 1.5f));

        for (var x = 4; x <= 7; x++)
        {
            if (x != 5)
                SpawnAttachedTo("WallWood", At(x + 0.5f, 4.5f));
            SpawnAttachedTo("WallWood", At(x + 0.5f, 7.5f));
        }

        for (var y = 5; y <= 6; y++)
        {
            SpawnAttachedTo("WallWood", At(4.5f, y + 0.5f));
            SpawnAttachedTo("WallWood", At(7.5f, y + 0.5f));
        }

        SpawnAttachedTo("WoodDoor", At(5.5f, 4.5f));
        SpawnAttachedTo("TableWood", At(5.5f, 6.5f));
        SpawnAttachedTo("MedkitFilled", At(6.5f, 6.5f));
        SpawnAttachedTo("MaterialWoodPlank10", At(2.5f, 0.5f));
        SpawnAttachedTo("SheetSteel10", At(3.5f, 0.5f));
        SpawnAttachedTo("CableApcStack10", At(3.5f, 1.5f));

        var paper = SpawnAttachedTo("Paper", At(6.5f, 5.5f));
        var note = EnsureComp<PaperComponent>(paper);
        note.Content = Loc.GetString("sstno-developer-test-note");
        note.EditingDisabled = true;
        Dirty(paper, note);
    }
}
