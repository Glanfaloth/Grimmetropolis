
public enum MapTileType
{
    Ground,
    Water,
    Stone
}

public class MapTile : TDComponent
{
    private MapTileType _type;

    public MapTile(TDObject tdObject, MapTileType type) : base(tdObject)
    {
        _type = type;
    }
}
