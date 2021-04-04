using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Map : TDComponent
{

    public MapTile[,] MapTiles { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }


    private int[,] _loadedMap;

    public Vector3 Corner { get; private set; }
    public Vector3 Offcenter { get; } = new Vector3(.5f, .5f, 0f);
    // TODO: change this to artifact location
    public Point EnemyTarget { get; set; }

    public List<MapDTO.EntityToSpawn> LoadFromFile(string fileName)
    {
        var result = new List<MapDTO.EntityToSpawn>();
        string jsonString = File.ReadAllText(fileName);
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        MapDTO mapDTO = JsonSerializer.Deserialize<MapDTO>(jsonString, options);

        Width = mapDTO.Map.Length;
        Height = mapDTO.Map[0].Length;
        Corner = new Vector3(-.5f * Width, -.5f * Height, 0);

        TDSceneManager.ActiveScene.CuboidColliderObjects = new TDCuboidCollider[Width, Height];
        _loadedMap = new int[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            if (mapDTO.Map[x].Length != Height)
            {
                throw new ArgumentException("Map isn't rectangular");
            }

            for (int y = 0; y < Height; y++)
            {
                MapDTO.LegendItem entry = mapDTO.Legend[mapDTO.Map[x][y].ToString()];
                _loadedMap[x, y] = (int)entry.TileType;

                if (entry.SpawnedEntity != MapDTO.EntityType.None)
                {
                    result.Add(new MapDTO.EntityToSpawn(new Point(x, y), entry.SpawnedEntity));
                }
            }
        }

        return result;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (_loadedMap == null)
        {
            throw new ArgumentException("No map was loaded");
        }

        LoadMap();
    }

    public override void Destroy()
    {
        base.Destroy();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TDSceneManager.ActiveScene.CuboidColliderObjects[x, y] = null;
            }
        }
    }

    // load map tiles
    private void LoadMap()
    {
        MapTiles = new MapTile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3 position = TDObject.Transform.LocalPosition + Corner + Offcenter + new Vector3(x, y, 0f);
                TDObject mapTileObject = (MapTileType)_loadedMap[x, y] switch
                {
                    MapTileType.Ground => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform),
                    MapTileType.Water => PrefabFactory.CreatePrefab(PrefabType.MapTileWater, position, Quaternion.Identity, TDObject.Transform),
                    _ => PrefabFactory.CreatePrefab(PrefabType.MapTileGround, position, Quaternion.Identity, TDObject.Transform)
                };

                MapTile mapTile = mapTileObject.GetComponent<MapTile>();
                mapTile.Position = new Point(x, y);

                MapTiles[x, y] = mapTile;
                TDSceneManager.ActiveScene.CuboidColliderObjects[x, y] = mapTile.collider;
            }
        }
    }

    public MapTile GetMapTile(Vector2 position)
    {
        int x = Math.Clamp((int)(position.X - TDObject.Transform.Position.X - Corner.X), 0, Width - 1);
        int y = Math.Clamp((int)(position.Y - TDObject.Transform.Position.Y - Corner.Y), 0, Height - 1);

        return MapTiles[x, y];
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
}