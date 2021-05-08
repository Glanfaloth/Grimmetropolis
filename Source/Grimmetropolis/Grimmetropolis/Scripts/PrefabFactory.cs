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
    BuildingWall,
    BuildingFarm,
    BuildingBridge,

    ResourceWood,
    ResourceStone,
    
    MagicalArtifact,

    ToolAxe,
    ToolPickaxe,

    WeaponSword,

    Arrow,
    StonePayload,
    Icicle,

    UIManager,

    EmptyUI,
    EmptyUI3D,

    ResourceDisplay,
    HealthBar,
    ProgressBar,
    BuildMenu,
    WaveIndicator,
    PlayerDisplay
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
                    // TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Player player = prefab.AddComponent<Player>();
                    CharacterAnimation animation = prefab.AddComponent<CharacterAnimation>();
                    animation.Character = player;
                    animation.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
                    animation.CharacterTexture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    player.Animation = animation;
                    /*mesh.Model = TDContentManager.LoadModel("PlayerCindarella");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");*/
                    collider.Radius = .25f;
                    collider.Height = .5f;
                    collider.Offset = .5f * Vector3.Backward;

                    TDObject interactionObject = CreatePrefab(PrefabType.Empty, 1f * Vector3.Right, Quaternion.Identity, prefab.Transform);
                    // TDMesh meshCollider = interactionObject.AddComponent<TDMesh>();
                    // meshCollider.Model = TDContentManager.LoadModel("DefaultCylinder");
                    // meshCollider.Texture = TDContentManager.LoadTexture("DefaultTexture");
                    TDCylinderCollider interactionCollider = interactionObject.AddComponent<TDCylinderCollider>();
                    interactionCollider.IsTrigger = true;
                    interactionCollider.Radius = .5f;
                    interactionCollider.Height = 2f;
                    interactionCollider.Offset = .5f * Vector3.Backward;
                    player.InteractionCollider = interactionCollider;
                    // player.Mesh = mesh;
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
                    mapTile.Mesh = mesh;
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
                    mapTile.Mesh = mesh;
                    break;
                }

            // Buildings
            case PrefabType.BuildingCastle:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Castle castle = prefab.AddComponent<Castle>();
                    mesh.Model = TDContentManager.LoadModel("BuildingCastle");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingCastleTexture");
                    castle.Size = new Point(3, 3);
                    castle.Mesh = mesh;
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
                    outpost.Mesh = mesh;
                    break;
                }

            case PrefabType.BuildingWall:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Wall wall = prefab.AddComponent<Wall>();
                    mesh.Model = TDContentManager.LoadModel("BuildingWall");
                    mesh.Texture = TDContentManager.LoadTexture("BuildingCastleTexture");
                    wall.Mesh = mesh;
                    break;
                }
            case PrefabType.BuildingFarm:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Farm farm = prefab.AddComponent<Farm>();
                    mesh.Model = TDContentManager.LoadModel("BuildingFarm");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    farm.Size = new Point(2, 2);
                    farm.Mesh = mesh;
                    break;
                }
            case PrefabType.BuildingBridge:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Bridge bridge = prefab.AddComponent<Bridge>();
                    mesh.Model = TDContentManager.LoadModel("BuildingBridge");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    bridge.Mesh = mesh;
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
                    resource.Mesh = mesh;
                    break;
                }

            case PrefabType.ResourceStone:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceDeposit resource = prefab.AddComponent<ResourceDeposit>();
                    mesh.Model = TDContentManager.LoadModel("ResourceStone");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resource.Type = ResourceDepositType.Stone;
                    resource.Mesh = mesh;
                    break;
                }

            case PrefabType.MagicalArtifact:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    MagicalArtifact magicalArtifact = prefab.AddComponent<MagicalArtifact>();
                    mesh.Model = TDContentManager.LoadModel("MagicalArtifact");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    magicalArtifact.Mesh = mesh;
                    break;
                }

            case PrefabType.ToolAxe:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ToolAxe axe = prefab.AddComponent<ToolAxe>();
                    mesh.Model = TDContentManager.LoadModel("ToolAxe");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    axe.Mesh = mesh;
                    break;
                }

            case PrefabType.ToolPickaxe:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ToolPickaxe pickaxe = prefab.AddComponent<ToolPickaxe>();
                    mesh.Model = TDContentManager.LoadModel("ToolPickaxe");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    pickaxe.Mesh = mesh;
                    break;
                }

            case PrefabType.WeaponSword:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    WeaponSword sword = prefab.AddComponent<WeaponSword>();
                    mesh.Model = TDContentManager.LoadModel("WeaponSword");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    sword.Mesh = mesh;
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
            case PrefabType.StonePayload:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Projectile projectile = prefab.AddComponent<Projectile>();
                    mesh.Model = TDContentManager.LoadModel("ProjectileStonePayload");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    projectile.Collider = collider;
                    break;
                }
            case PrefabType.Icicle:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    Projectile projectile = prefab.AddComponent<Projectile>();
                    mesh.Model = TDContentManager.LoadModel("ProjectileIcicle");
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

                    TDObject woodObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDObject stoneObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDObject foodObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite wood = woodObject.AddComponent<TDSprite>();
                    TDSprite stone = stoneObject.AddComponent<TDSprite>();
                    TDSprite food = foodObject.AddComponent<TDSprite>();
                    wood.Texture = TDContentManager.LoadTexture("UIWood");
                    stone.Texture = TDContentManager.LoadTexture("UIStone");
                    food.Texture = TDContentManager.LoadTexture("UIFood");
                    resourceDisplay.WoodUI = wood;
                    resourceDisplay.StoneUI = stone;
                    resourceDisplay.FoodUI = food;
                    woodObject.RectTransform.Scale = 0.05f * Vector2.One;
                    woodObject.RectTransform.LocalPosition = new Vector2(-2f, 10f);
                    stoneObject.RectTransform.Scale = 0.04f * Vector2.One;
                    stoneObject.RectTransform.LocalPosition = new Vector2(5f + 0.05f * wood.Texture.Width, 10f);
                    foodObject.RectTransform.Scale = 0.06f * Vector2.One;
                    foodObject.RectTransform.LocalPosition = new Vector2(0.05f * wood.Texture.Width + 0.06f * stone.Texture.Width, 25f);
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

            case PrefabType.PlayerDisplay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDSprite playerIcon = prefab.AddComponent<TDSprite>();
                    PlayerDisplay playerDisplay = prefab.AddComponent<PlayerDisplay>();
                    playerIcon.Texture = TDContentManager.LoadTexture("UIPlayer");
                    prefab.RectTransform.Origin = new Vector2(.5f * playerIcon.Texture.Width, playerIcon.Texture.Height);

                    TDObject healthBarObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    HealthBar healthBar = healthBarObject.AddComponent<HealthBar>();
                    TDSprite background = healthBarObject.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    healthBarObject.RectTransform.LocalPosition = new Vector2(.5f * playerIcon.Texture.Width, -.8f * playerIcon.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, healthBarObject.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    foreground.Depth = .1f;
                    healthBar.Background = background;
                    healthBar.Foreground = foreground;
                    healthBar.AlwaysShow = true;
                    playerDisplay.HealthBar = healthBar;
                    break;
                }

            case PrefabType.BuildMenu:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite buildSprite = prefab.AddComponent<TDSprite>();
                    BuildMenu buildMenu = prefab.AddComponent<BuildMenu>();
                    buildSprite.Texture = TDContentManager.LoadTexture("UIBuild");
                    buildSprite.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(0f, buildSprite.Texture.Height);

                    TDObject iconObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite icon = iconObject.AddComponent<TDSprite>();
                    icon.Texture = TDContentManager.LoadTexture("UIBuildingOutpostIcon");
                    iconObject.RectTransform.Origin = .5f * new Vector2(icon.Texture.Width, icon.Texture.Height);
                    iconObject.RectTransform.LocalPosition = -prefab.RectTransform.Origin + .5f * new Vector2(buildSprite.Texture.Width, buildSprite.Texture.Height);
                    buildMenu.BuildSprite = buildSprite;
                    buildMenu.Icon = icon;
                    break;
                }

            case PrefabType.WaveIndicator:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDSprite testImage = prefab.AddComponent<TDSprite>();
                    WaveIndicator waveIndicator = prefab.AddComponent<WaveIndicator>();
                    testImage.Texture = TDContentManager.LoadTexture("UIPlayer");
                    testImage.Depth = 1f;
                    waveIndicator.Image = testImage;
                    prefab.RectTransform.Origin = new Vector2(testImage.Texture.Width, 0f);

                    TDObject textInfoObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText testText = textInfoObject.AddComponent<TDText>();
                    testText.Text = "Hello, world! This is a test!";
                    testImage.Depth = .9f;
                    textInfoObject.RectTransform.LocalPosition = new Vector2(-(testImage.Texture.Width + testText.Width + 10f), 0f);
                    break;
                }
        }

        return prefab;
    }

    internal static TDObject CreateEnemyPrefab<T>(Config.EnemyStats stats, Vector3 localPosition, Quaternion localRotation, TDTransform parent = null) where T : Enemy, new()
    {
        TDObject prefab = CreatePrefab(PrefabType.Empty, localPosition, localRotation, parent);
        TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
        T enemy = prefab.AddComponent<T>();
        enemy.SetBaseStats(stats);
        if (enemy is EnemyKnight)
        {
            CharacterAnimation animation = prefab.AddComponent<CharacterAnimation>();
            animation.Character = enemy;
            enemy.Animation = animation;
            animation.CharacterModel = TDContentManager.LoadModel(enemy.MeshName);
            animation.CharacterTexture = TDContentManager.LoadTexture("ColorPaletteTexture");
        }
        else if (enemy is EnemyCatapult)
        {
            CatapultAnimation animation = prefab.AddComponent<CatapultAnimation>();
            animation.Character = enemy;
            enemy.Animation = animation;
            animation.CharacterModel = TDContentManager.LoadModel(enemy.MeshName);
            animation.CharacterTexture = TDContentManager.LoadTexture("ColorPaletteTexture");
        }
        else
        {
            TDMesh mesh = prefab.AddComponent<TDMesh>();
            mesh.Model = TDContentManager.LoadModel(enemy.MeshName);
            mesh.Texture = TDContentManager.LoadTexture(enemy.TextureName);
            enemy.Mesh = mesh;
        }
        if (enemy is EnemyCatapult) collider.Radius = .375f;
        else collider.Radius = .25f;
        collider.Height = .5f;
        collider.Offset = .5f * Vector3.Backward;

        TDObject interactionObject = CreatePrefab(PrefabType.Empty, 1f * Vector3.Right, Quaternion.Identity, prefab.Transform);
        // TDMesh meshCollider = interactionObject.AddComponent<TDMesh>();
        // meshCollider.Model = TDContentManager.LoadModel("DefaultCylinder");
        // meshCollider.Texture = TDContentManager.LoadTexture("DefaultTexture");
        TDCylinderCollider interactionCollider = interactionObject.AddComponent<TDCylinderCollider>();
        interactionCollider.IsTrigger = true;
        interactionCollider.Radius = .5f;
        interactionCollider.Height = 2f;
        interactionCollider.Offset = .5f * Vector3.Backward;
        enemy.InteractionCollider = interactionCollider;

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
