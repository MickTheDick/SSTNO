namespace Content.Server.SSTNO;

/// <summary>
/// Marks a station as the SSTNO developer test pad.
/// The loaded placeholder grid is retiled as grass and given air, gravity, and a small camp.
/// </summary>
[RegisterComponent]
public sealed partial class SstnoDeveloperTestAreaComponent : Component
{
    /// <summary>
    /// Fixed daylight color applied to the map.
    /// </summary>
    [DataField]
    public Color MapLightColor = Color.FromHex("#D8B059");

    /// <summary>
    /// Half-size, in tiles, of the grass pad.
    /// </summary>
    [DataField]
    public int PadRadius = 16;

    [DataField]
    public string GrassTile = "FloorPlanetGrass";
}
