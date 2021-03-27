using Microsoft.Xna.Framework;

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
    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        TDObject prefab = new TDObject(localPosition, localRotation, localScale, parent);

        switch (type)
        {
            // Basic prefabs
            case PrefabType.Camera:
                {
                    TDCamera camera = prefab.AddComponent<TDCamera>();
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
                    break;
                }

            case PrefabType.Enemy:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    prefab.AddComponent<Enemy>();
                    mesh.Model = TDContentManager.LoadModel("EnemyWitch");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    collider.Radius = .25f;
                    collider.Height = .5f;
                    collider.Offset = .5f * Vector3.Backward;
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
            case PrefabType.BuildingOutpost:
                {
                    prefab.AddComponent<TDMesh>();
                    prefab.AddComponent<TDCuboidCollider>();
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    mesh.Model = TDContentManager.LoadModel("BuildingOutpost");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingOutpostTexture");
                    collider.Size = new Vector3(1f, 1f, 2f);
                    collider.Offset = Vector3.Backward;
                    break;
                }
        }

        return prefab;
    }

    public static TDObject CreatePrefab(PrefabType type)
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
}
