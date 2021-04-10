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
    // Enemy,

    MapTileGround,
    MapTileWater,

    BuildingCastle,
    BuildingOutpost,

    ResourceWood,
    ResourceStone,

    ToolAxe,
    ToolPickaxe,

    Arrow,

    UIManager,

    EmptyUI,
    EmptyUI3D,

    ResourceDisplay,
    HealthBar,
    ProgressBar
}

public static class PrefabFactory
{
    public static EnemyController EnemyController { get; private set; }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent = null)
    {
        TDObject prefab = new TDObject(localPosition, localRotation, localScale, parent);

        switch (type)
        {
            // Basic prefabs
            case PrefabType.Camera:
                {
                    prefab.AddComponent<TDCamera>();
                    prefab.AddComponent<CameraToPlayers>();
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

            //case PrefabType.Enemy:
            //    {
            //        TDMesh mesh = prefab.AddComponent<TDMesh>();
            //        TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
            //        Enemy enemy = prefab.AddComponent<Enemy>();
            //        mesh.Model = TDContentManager.LoadModel("EnemyCatapult");
            //        mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
            //        collider.Radius = .25f;
            //        collider.Height = .5f;
            //        collider.Offset = .5f * Vector3.Backward;

            //        TDObject interactionObject = CreatePrefab(PrefabType.Empty, 1f * Vector3.Right, Quaternion.Identity, prefab.Transform);
            //        // TDMesh meshCollider = interactionObject.AddComponent<TDMesh>();
            //        // meshCollider.Model = TDContentManager.LoadModel("DefaultCylinder");
            //        // meshCollider.Texture = TDContentManager.LoadTexture("DefaultTexture");
            //        TDCylinderCollider interactionCollider = interactionObject.AddComponent<TDCylinderCollider>();
            //        interactionCollider.IsTrigger = true;
            //        interactionCollider.Radius = .25f;
            //        interactionCollider.Height = .5f;
            //        interactionCollider.Offset = .5f * Vector3.Backward;
            //        enemy.InteractionCollider = interactionCollider;
            //        break;
            //    }

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
            case PrefabType.BuildingCastle:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    prefab.AddComponent<Castle>();
                    mesh.Model = TDContentManager.LoadModel("BuildingCastle");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingCastleTexture");
                    break;
                }

            case PrefabType.BuildingOutpost:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Outpost outpost = prefab.AddComponent<Outpost>();
                    mesh.Model = TDContentManager.LoadModel("BuildingOutpost");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingOutpostTexture");

                    TDObject shootingObject = CreatePrefab(PrefabType.Empty, prefab.Transform);
                    TDCylinderCollider shootingRange = shootingObject.AddComponent<TDCylinderCollider>();
                    outpost.ShootingRange = shootingRange;
                    break;
                }

            // Resources
            case PrefabType.ResourceWood:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceDeposit resource = prefab.AddComponent<ResourceDeposit>();
                    mesh.Model = TDContentManager.LoadModel("ResourceWood");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resource.Type = ResourceDepositType.Wood;
                    break;
                }

            case PrefabType.ResourceStone:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceDeposit resource = prefab.AddComponent<ResourceDeposit>();
                    mesh.Model = TDContentManager.LoadModel("ResourceStone");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resource.Type = ResourceDepositType.Stone;
                    break;
                }

            case PrefabType.ToolAxe:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    prefab.AddComponent<ToolAxe>();
                    mesh.Model = TDContentManager.LoadModel("ToolAxe");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    break;
                }

            case PrefabType.ToolPickaxe:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    prefab.AddComponent<ToolPickaxe>();
                    mesh.Model = TDContentManager.LoadModel("ToolPickaxe");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    break;
                }

            // Projectiles
            case PrefabType.Arrow:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Projectile projectile = prefab.AddComponent<Projectile>();
                    mesh.Model = TDContentManager.LoadModel("ProjectileArrow");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    projectile.Collider = collider;
                    break;
                }

            // UI
            case PrefabType.UIManager:
                {
                    prefab.AddComponent<UIManager>();
                    break;
                }

            case PrefabType.EmptyUI:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    break;
                }

            case PrefabType.EmptyUI3D:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    break;
                }

            case PrefabType.ResourceDisplay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDText text = prefab.AddComponent<TDText>();
                    ResourceDisplay resourceDisplay = prefab.AddComponent<ResourceDisplay>();
                    resourceDisplay.TextUI = text;
                    break;
                }

            case PrefabType.HealthBar:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    HealthBar healthBar = prefab.AddComponent<HealthBar>();
                    background.Texture = TDContentManager.LoadTexture("UIBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * background.Texture.Width, background.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIBar");
                    foreground.Depth = .1f;
                    healthBar.Background = background;
                    healthBar.Foreground = foreground;
                    foregroundObject.RectTransform.LocalPosition = -prefab.RectTransform.Origin;
                    break;
                }

            case PrefabType.ProgressBar:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    ProgressBar progressBar = prefab.AddComponent<ProgressBar>();
                    background.Texture = TDContentManager.LoadTexture("UIBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * background.Texture.Width, background.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIBar");
                    foreground.Depth = .1f;
                    progressBar.Background = background;
                    progressBar.Foreground = foreground;
                    foregroundObject.RectTransform.LocalPosition = -prefab.RectTransform.Origin;
                    break;
                }
        }

        return prefab;
    }

    internal static TDObject CreateEnemyPrefab<T>(Config.EnemyStats stats, Vector3 localPosition, Quaternion localRotation, TDTransform parent = null) where T : Enemy, new()
    {
        TDObject prefab = CreatePrefab(PrefabType.Empty, localPosition, localRotation, parent);
        TDMesh mesh = prefab.AddComponent<TDMesh>();
        TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
        T enemy = prefab.AddComponent<T>();
        enemy.SetBaseStats(stats);
        mesh.Model = TDContentManager.LoadModel(enemy.MeshName);
        mesh.Texture = TDContentManager.LoadTexture(enemy.TextureName);
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
        enemy.InteractionCollider = interactionCollider;

        if (stats.ATTACK_RANGE > .25f)
        {
            TDObject shootingObject = CreatePrefab(PrefabType.Empty, prefab.Transform);
            TDCylinderCollider shootingRange = shootingObject.AddComponent<TDCylinderCollider>();
            enemy.ShootingRange = shootingRange;
        }

        return prefab;
    }

    public static TDObject CreatePrefab(PrefabType type, TDTransform parent = null)
    {
        return CreatePrefab(type, Vector3.Zero, Quaternion.Identity, Vector3.One, parent);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, TDTransform parent = null)
    {
        return CreatePrefab(type, localPosition, Quaternion.Identity, Vector3.One, parent);
    }

    public static TDObject CreatePrefab(PrefabType type, Vector3 localPosition, Quaternion localRotation, TDTransform parent = null)
    {
        return CreatePrefab(type, localPosition, localRotation, Vector3.One, parent);
    }

    public static void CreateEmptyUI(TDObject prefab, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        prefab.RectTransform = new TDRectTransform
        {
            TDObject = prefab,
            Parent = prefab.Transform.Parent?.TDObject.RectTransform,
            LocalPosition = new Vector2(localPosition.X, localPosition.Y),
            LocalRotation = 2f * MathF.Asin(localRotation.Z),
            LocalScale = new Vector2(localScale.X, localScale.Y)
        };
    }

    public static void CreateEmptyUI3D(TDObject prefab, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        prefab.RectTransform = new TDRectTransform
        {
            TDObject = prefab,
            Parent3D = prefab.Transform.Parent
        };
    }
}
