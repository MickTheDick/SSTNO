using Content.IntegrationTests.Fixtures;
using Content.Server.GameTicking;
using Content.Server.Shuttles.Components;
using Content.Server.SSTNO;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Prototypes;
using Content.Shared.Gravity;
using Content.Shared.Maps;
using Content.Shared.Shuttles.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.SSTNO;

/// <summary>
/// Proves the SSTNO developer baseline starts on the outdoor test area
/// and does not attach station shuttle or central-command components.
/// </summary>
[TestFixture]
public sealed class SstnoDeveloperRoundTest : GameTest
{
    public override PoolSettings PoolSettings => new()
    {
        Dirty = true,
        DummyTicker = false,
        Connected = true,
        InLobby = true,
        Map = "SSTNODev",
    };

    private static readonly ProtoId<GamePresetPrototype> PresetId = "SSTNO";
    private static readonly ProtoId<GameMapPrototype> DevMapId = "SSTNODev";
    private static readonly EntProtoId RoundRuleId = "SSTNORound";

    [Test]
    public async Task RoundStartsOnDeveloperTestArea()
    {
        var pair = Pair;
        var server = pair.Server;
        var ticker = server.System<ServerGameTicker>();
        var mapSystem = server.System<SharedMapSystem>();
        var prototypes = server.ResolveDependency<IPrototypeManager>();

        Assert.That(prototypes.TryIndex(PresetId, out var preset), Is.True);
        Assert.That(preset!.ShowInVote, Is.True);
        Assert.That(preset.Rules, Does.Contain(RoundRuleId));
        Assert.That(prototypes.Index(DevMapId).MapName, Is.EqualTo("SSTNO Developer Test Area"));

        await server.WaitPost(() =>
        {
            Assert.That(ticker.RunLevel, Is.EqualTo(GameRunLevel.PreRoundLobby));
        });

        await pair.WaitClientCommand("toggleready True");
        await pair.WaitCommand("setgamepreset SSTNO 9999");
        await pair.WaitCommand("startround");
        await pair.RunTicksSync(20);

        Assert.That(ticker.RunLevel, Is.EqualTo(GameRunLevel.InRound));

        var player = pair.Player!.AttachedEntity;
        Assert.That(player, Is.Not.Null);
        Assert.That(server.EntMan.EntityExists(player!.Value), Is.True);

        var xform = server.Transform(player.Value);
        Assert.That(xform.MapUid, Is.Not.Null);
        Assert.That(xform.MapID, Is.Not.EqualTo(MapId.Nullspace));
        Assert.That(xform.GridUid, Is.Not.Null);
        Assert.That(server.EntMan.HasComponent<GravityComponent>(xform.GridUid!.Value), Is.True);
        Assert.That(server.EntMan.HasComponent<ShuttleComponent>(xform.GridUid.Value), Is.False);

        var grid = server.EntMan.GetComponent<MapGridComponent>(xform.GridUid!.Value);
        var tile = mapSystem.GetTileRef(xform.GridUid.Value, grid, xform.Coordinates);
        Assert.That(tile.Tile.IsEmpty, Is.False, "Player spawned on an empty tile.");

        var foundStation = false;
        var stations = server.EntMan.EntityQueryEnumerator<SstnoDeveloperTestAreaComponent>();
        while (stations.MoveNext(out var station, out _))
        {
            foundStation = true;
            Assert.That(server.EntMan.HasComponent<StationEmergencyShuttleComponent>(station), Is.False);
            Assert.That(server.EntMan.HasComponent<StationCentcommComponent>(station), Is.False);
            Assert.That(server.EntMan.HasComponent<StationArrivalsComponent>(station), Is.False);
        }

        Assert.That(foundStation, Is.True);
        Assert.That(server.EntMan.Count<StationCentcommComponent>(), Is.Zero);
        Assert.That(server.EntMan.Count<StationEmergencyShuttleComponent>(), Is.Zero);

        var meta = server.EntMan.GetComponent<MetaDataComponent>(player.Value);
        Assert.That(meta.EntityPrototype?.ID, Is.EqualTo("MobHuman"));
    }
}
