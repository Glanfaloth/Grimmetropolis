using Microsoft.Xna.Framework;
using System;

public enum PrefabType
{
    Empty,
    Camera,
    Light,
    Default,
    Player,
    Enemy,
    MapTileGround,
    MapTileWater,
    MapTileStone,
    BuildingOutpost
}

public static class PrefabFactory
{
    public static EnemyController EnemyController { get; private set; }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent, bool updateFirst = false)
    {
        TDObject prefab = new TDObject(localPosition, localRotation, localScale, parent, updateFirst);

        switch (type)
        {
            // Basic prefabs
            case PrefabType.Camera:
                prefab.Components.Add(new TDCamera(prefab, MathHelper.PiOver2, 2f, 20f));
                break;

            case PrefabType.Light:
                prefab.Components.Add(new TDLight(prefab, MathHelper.PiOver2, 2f, 20f));
                break;

            case PrefabType.Default:
                prefab.Components.Add(new TDMesh(prefab, "DefaultModel", "DefaultTexture"));
                break;

            // Characters
            case PrefabType.Player:
                prefab.Components.Add(new TDMesh(prefab, "PlayerCindarella", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCylinderCollider(prefab, false, .25f, .5f, .5f * Vector3.Backward));
                prefab.Components.Add(new Player(prefab, 0f));
                break;

            case PrefabType.Enemy:
                prefab.Components.Add(new TDMesh(prefab, "EnemyWitch", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCylinderCollider(prefab, false, .25f, .5f, .5f * Vector3.Backward));
                prefab.Components.Add(new Enemy(prefab, 0f, EnemyController));
                break;

            // Map tiles
            case PrefabType.MapTileGround:
                prefab.Components.Add(new TDMesh(prefab, "MapTileGround", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCuboidCollider(prefab, false, Vector3.One, .5f * Vector3.Forward));
                prefab.Components.Add(new MapTile(prefab, MapTileType.Ground, new Vector2(localPosition.X, localPosition.Y)));
                break;

            case PrefabType.MapTileWater:
                prefab.Components.Add(new TDMesh(prefab, "MapTileWater", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCuboidCollider(prefab, false, new Vector3(1f, 1f, 2f), Vector3.Zero));
                prefab.Components.Add(new MapTile(prefab, MapTileType.Water, new Vector2(localPosition.X, localPosition.Y)));
                break;

            case PrefabType.MapTileStone:
                prefab.Components.Add(new TDMesh(prefab, "MapTileStone", "ColorPaletteTexture"));
                prefab.Components.Add(new TDCuboidCollider(prefab, false, Vector3.One, .5f * Vector3.Forward));
                prefab.Components.Add(new MapTile(prefab, MapTileType.Stone, new Vector2(localPosition.X, localPosition.Y)));
                break;

            // Buildings
            case PrefabType.BuildingOutpost:
                prefab.Components.Add(new TDMesh(prefab, "BuildingOutpost", "BuildingOutpostTexture"));
                prefab.Components.Add(new TDCuboidCollider(prefab, false, new Vector3(1f, 1f, 2f), Vector3.Zero));
                break;
        }

        return prefab;
    }

    public static TDObject CreatePrefab(PrefabType type, bool updateFirst = false)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, null, updateFirst);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation)
    {
        return CreatePrefab(type, localPosition, localRotation, Vector3.One, null);
    }

    public static TDObject CreatePrefab(PrefabType type, TDTransform parent)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, parent);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, TDTransform parent)
    {
        return CreatePrefab(type, localPosition, localRotation, Vector3.One, parent);
    }

    public static void InitializeEnemyController(Map mapComponent)
    {
        TDObject enemyAI = CreatePrefab(PrefabType.Empty, updateFirst: true);
        EnemyController = new EnemyController(enemyAI, mapComponent);
        enemyAI.Components.Add(EnemyController);
    }
}
