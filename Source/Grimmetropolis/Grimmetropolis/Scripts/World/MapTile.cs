
public enum MapTileType
{
    Ground,
    Water,
    Stone
}

public class MapTile : TDComponent
{
    public MapTileType Type;

    public override void Initialize()
    {
        base.Initialize();

        Type = MapTileType.Ground;
    }
}
