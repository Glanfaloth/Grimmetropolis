
using Microsoft.Xna.Framework;
using System;

public enum MapTileType
{
    Ground,
    Water
}

public class MapTile : TDComponent
{
    public Point Position = Point.Zero;
    public MapTileType Type = MapTileType.Ground;

    public bool IsPassable = true;

    public Structure Structure = null;

    public bool CanEnemyMoveThrough()
    {
        if (Structure != null)
        {
            return Structure.IsPassable;
        }

        return IsPassable;
    }
}
