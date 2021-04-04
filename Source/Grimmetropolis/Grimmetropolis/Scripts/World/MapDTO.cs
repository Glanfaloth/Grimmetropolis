using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class MapDTO
{
    public class LegendItem
    {
        public MapTileType TileType { get; set; }
        public EntityType SpawnedEntity { get; set; }
    }

    public enum EntityType
    {
        None,
        WoodResource,
        StoneResource,
        Castle,
        Enemy,
        Outpost,
        EnemySpawnPoint,
    }

    public Dictionary<string, LegendItem> Legend { get; set; } = new Dictionary<string, LegendItem>();
    public string[] Map { get; set; }

    public class EntityToSpawn
    {
        public Point Position { get; }
        public EntityType Type { get; }

        public EntityToSpawn(Point position, EntityType type)
        {
            Position = position;
            Type = type;
        }
    }
}
