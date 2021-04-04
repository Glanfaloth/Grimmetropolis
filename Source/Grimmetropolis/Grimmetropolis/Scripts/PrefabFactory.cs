using Microsoft.Xna.Framework;

using System;

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

    Castle,
    Outpost,

    Wood,
    Stone,

    Empty2D
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
                    // TDMesh meshCollider = interactionObject.AddComponent<TDMesh>();
                    // meshCollider.Model = TDContentManager.LoadModel("DefaultCylinder");
                    // meshCollider.Texture = TDContentManager.LoadTexture("DefaultTexture");
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
                    mapTile.Type = MapTileType.Ground;
                    mapTile.collider = collider;
                    break;
                }

            case PrefabType.MapTileWater:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCuboidCollider collider = prefab.AddComponent<TDCuboidCollider>();
                    MapTile mapTile = prefab.AddComponent<MapTile>();
                    mesh.Model = TDContentManager.LoadModel("MapTileWater");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    mapTile.Type = MapTileType.Water;
                    mapTile.collider = collider;
                    break;
                }

            // Buildings
            case PrefabType.Castle:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    prefab.AddComponent<Castle>();
                    mesh.Model = TDContentManager.LoadModel("BuildingCastle");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingCastleTexture");
                    break;
                }

            case PrefabType.Outpost:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Outpost outpost = prefab.AddComponent<Outpost>();
                    mesh.Model = TDContentManager.LoadModel("BuildingOutpost");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingOutpostTexture");

                    TDObject shootingObject = CreatePrefab(PrefabType.Empty, prefab.Transform);
                    TDCylinderCollider shootingRange = shootingObject.AddComponent<TDCylinderCollider>();
                    shootingRange.IsTrigger = true;
                    shootingRange.Radius = 3f;
                    shootingRange.Height = 1f;
                    shootingRange.Offset = Vector3.Zero;
                    outpost.ShootingRange = shootingRange;
                    break;
                }

            // Resources
            case PrefabType.Wood:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceDeposit resource = prefab.AddComponent<ResourceDeposit>();
                    mesh.Model = TDContentManager.LoadModel("ResourceWood");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resource.Type = ResourceDepositType.Wood;
                    break;
                }

            case PrefabType.Stone:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceDeposit resource = prefab.AddComponent<ResourceDeposit>();
                    mesh.Model = TDContentManager.LoadModel("ResourceStone");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resource.Type = ResourceDepositType.Stone;
                    break;
                }

            // UI
            case PrefabType.Empty2D:
                {
                    prefab.RectTransform = new TDRectTransform
                    {
                        Parent = prefab.Transform.Parent?.TDObject.RectTransform,
                        LocalPosition = new Vector2(localPosition.X, localPosition.Y),
                        LocalRotation = 2f * MathF.Asin(localRotation.Z),
                        LocalScale = new Vector2(localScale.X, localScale.Y)
                    };
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
