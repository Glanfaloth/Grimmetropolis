
using Microsoft.Xna.Framework;
using System;

public enum MapTileType
{
    Ground,
    Water,
    Stone
}

public class MapTile : TDComponent
{
    private MapTileType _type;

    public Vector2 Position { get; }

    public MapTile(TDObject tdObject, MapTileType type, Vector2 position) : base(tdObject)
    {
        _type = type;
        Position = position;
    }

    public bool CanEnemyMoveThrough()
    {
        // TODO: how do we check for buildings?

        switch (_type)
        {
            case MapTileType.Ground:
                return true;
            case MapTileType.Water:
                return false;
            case MapTileType.Stone:
                return true;
            default:
                throw new NotSupportedException();
        }
    }
}
