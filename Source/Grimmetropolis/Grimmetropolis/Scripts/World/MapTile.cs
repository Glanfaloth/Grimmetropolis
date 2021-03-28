
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

    public bool IsPassable = true;

    /*public MapTile(MapTileType type, Vector2 position)
    {
        Type = type;
        Position = position;
    }*/

    public bool CanEnemyMoveThrough()
    {
        // TODO: how do we check for buildings?

        if (Building != null)
        {
            return Building.IsPassable;
        }

        return IsPassable;
    }
}
