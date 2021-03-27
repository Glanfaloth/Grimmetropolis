
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
    public MapTileType Type = MapTileType.Ground;

    public Point Position;

    public Building Building;

    /*public MapTile(MapTileType type, Vector2 position)
    {
        Type = type;
        Position = position;
    }*/

    public bool CanEnemyMoveThrough()
    {
        // TODO: how do we check for buildings?

        switch (Type)
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
