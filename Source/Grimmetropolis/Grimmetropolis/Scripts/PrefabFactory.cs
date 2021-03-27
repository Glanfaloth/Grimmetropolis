using Microsoft.Xna.Framework;

using System.Diagnostics;

public enum PrefabType
{
    Empty,
    Camera,
    Light,
    Default,
    GameManager,
    Player,
    Enemy,
    MapTileGround,
    MapTileWater,
    MapTileStone,
    Outpost
}

public static class PrefabFactory
{
    public static EnemyController EnemyController { get; private set; }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        TDObject prefab = new TDObject(localPosition, localRotation, localScale, parent);

        switch (type)
        {
            // Basic prefabs
            case PrefabType.Camera:
                {
                    prefab.AddComponent<TDCamera>();
                    break;
                }

            case PrefabType.Light:
                {
                    prefab.AddComponent<TDLight>();
                    break;
                }

            case PrefabType.Default:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    mesh.Model = TDContentManager.LoadModel("DefaultModel");
                    mesh.Texture = TDContentManager.LoadTexture("DefaultTexture");
                    break;
                }

            // GameManager
            case PrefabType.GameManager:
                {
                    prefab.AddComponent<GameManager>();
                    break;
                }

            // Characters
            case PrefabType.Player:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Player player = prefab.AddComponent<Player>();
                    mesh.Model = TDContentManager.LoadModel("PlayerCindarella");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Radius = .25f;
                    collider.Height = .5f;
                    collider.Offset = .5f * Vector3.Backward;

                    TDObject interactionObject = CreatePrefab(PrefabType.Empty, 1f * Vector3.Right, Quaternion.Identity, prefab.Transform);
                    TDMesh meshCollider = interactionObject.AddComponent<TDMesh>();
                    meshCollider.Model = TDContentManager.LoadModel("DefaultCylinder");
                    meshCollider.Texture = TDContentManager.LoadTexture("DefaultTexture");
                    TDCylinderCollider interactionCollider = interactionObject.AddComponent<TDCylinderCollider>();
                    interactionCollider.IsTrigger = true;
                    interactionCollider.Radius = .25f;
                    interactionCollider.Height = .5f;
                    interactionCollider.Offset = .5f * Vector3.Backward;
                    player.InteractionCollider = interactionCollider;
                    break;
                }

            case PrefabType.Enemy:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Enemy enemy = prefab.AddComponent<Enemy>();
                    mesh.Model = TDContentManager.LoadModel("EnemyWitch");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Radius = .25f;
                    collider.Height = .5f;
                    collider.Offset = .5f * Vector3.Backward;

                    TDObject interactionObject = CreatePrefab(PrefabType.Empty, 1f * Vector3.Right, Quaternion.Identity, prefab.Transform);
                    TDCylinderCollider interactionCollider = interactionObject.AddComponent<TDCylinderCollider>();
                    interactionCollider.IsTrigger = true;
                    interactionCollider.Radius = .25f;
                    interactionCollider.Height = .5f;
                    interactionCollider.Offset = .5f * Vector3.Backward;
                    enemy.InteractionCollider = interactionCollider;
                    break;
                }

            // Map tiles
            case PrefabType.MapTileGround:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    MapTile mapTile = prefab.AddComponent<MapTile>();
                    mesh.Model = TDContentManager.LoadModel("MapTileGround");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Size = Vector3.One;
                    collider.Offset = .5f * Vector3.Forward;
                    mapTile.Type = MapTileType.Ground;
                    break;
                }

            case PrefabType.MapTileWater:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    MapTile mapTile = prefab.AddComponent<MapTile>();
                    mesh.Model = TDContentManager.LoadModel("MapTileWater");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Size = new Vector3(1f, 1f, 2f);
                    collider.Offset = Vector3.Zero;
                    mapTile.Type = MapTileType.Water;
                    break;
                }

            case PrefabType.MapTileStone:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    MapTile mapTile = prefab.AddComponent<MapTile>();
                    mesh.Model = TDContentManager.LoadModel("MapTileStone");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Size = Vector3.One;
                    collider.Offset = .5f * Vector3.Forward;
                    mapTile.Type = MapTileType.Stone;
                    break;
                }

            // Buildings
            case PrefabType.Outpost:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    prefab.AddComponent<Outpost>();
                    mesh.Model = TDContentManager.LoadModel("BuildingOutpost");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingOutpostTexture");
                    collider.Size = new Vector3(1f, 1f, 2f);
                    collider.Offset = Vector3.Backward;
                    break;
                }
        }

        return prefab;
    }

    public static TDObject CreatePrefab(PrefabType type, bool updateFirst = false)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, null);
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

    /*public static void InitializeEnemyController(Map mapComponent)
    {
        TDObject enemyAI = CreatePrefab(PrefabType.Empty, updateFirst: true);
        EnemyController = new EnemyController(mapComponent);
        enemyAI.AddComponent(EnemyController);
    }*/
}
