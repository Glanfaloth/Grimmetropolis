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
    PlayerPreview,
    // Enemy,
    TutorialGuy,

    MapTileGround,
    MapTileWater,

    BuildingCastle,
    BuildingOutpost,
    BuildingWall,
    BuildingFarm,
    BuildingBridge,
    BuildingHospital,
    BuildingResourceBuilding,

    ResourceWood,
    ResourceStone,
    
    MagicalArtifact,

    ToolAxe,
    ToolPickaxe,
    ToolHammer,

    WeaponSword,

    Arrow,
    StonePayload,
    Icicle,

    UIManager,
    MenuUIManager,

    EmptyUI,
    EmptyUI3D,

    ResourceDisplay,
    HealthBar,
    ProgressBar,
    WaveBar,
    BuildMenu,
    WaveIndicator,
    PlayerDisplay,
    GameOverOverlay,

    MainMenu,
    CharacterDisplay
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
                    player.Collider = collider;
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

            case PrefabType.PlayerPreview:
                {
                    CharacterAnimation animation = prefab.AddComponent<CharacterAnimation>();
                    animation.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
                    animation.CharacterTexture = TDContentManager.LoadTexture("ColorPaletteTexture");
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
            case PrefabType.TutorialGuy:
                {
                    TDCylinderCollider collider = prefab.AddComponent<TDCylinderCollider>();
                    TutorialAdvicer tutorialGuy = prefab.AddComponent<TutorialAdvicer>();
                    tutorialGuy.SetBaseStats(Config.TUTORIAL_GUY_STATS);

                    CharacterAnimation animation = prefab.AddComponent<CharacterAnimation>();
                    animation.Character = tutorialGuy;
                    tutorialGuy.Animation = animation;
                    animation.CharacterModel = TDContentManager.LoadModel(tutorialGuy.MeshName);
                    animation.CharacterTexture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    
                    collider.Radius = .25f;
                    tutorialGuy.Collider = collider;
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
                    tutorialGuy.InteractionCollider = interactionCollider;

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
            case PrefabType.BuildingHospital:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    Hospital hospital = prefab.AddComponent<Hospital>();
                    mesh.Model = TDContentManager.LoadModel("BuildingHospital");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    hospital.Size = new Point(2, 2);
                    hospital.Mesh = mesh;

                    TDObject interactionColliderObject = CreatePrefab(PrefabType.Empty, prefab.Transform);
                    TDCylinderCollider interactionCollider = interactionColliderObject.AddComponent<TDCylinderCollider>();
                    hospital.InteractionCollider = interactionCollider;
                    break;
                }
            case PrefabType.BuildingResourceBuilding:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ResourceBuilding resourceBuilding = prefab.AddComponent<ResourceBuilding>();
                    mesh.Model = TDContentManager.LoadModel("BuildingResourceBuilding");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    resourceBuilding.Size = new Point(2, 2);
                    resourceBuilding.Mesh = mesh;

                    TDObject interactionColliderObject = CreatePrefab(PrefabType.Empty, prefab.Transform);
                    TDCylinderCollider interactionCollider = interactionColliderObject.AddComponent<TDCylinderCollider>();
                    resourceBuilding.InteractionCollider = interactionCollider;
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

            case PrefabType.ToolHammer:
                {
                    TDMesh mesh = prefab.AddComponent<TDMesh>();
                    ToolHammer hammer = prefab.AddComponent<ToolHammer>();
                    mesh.Model = TDContentManager.LoadModel("ToolHammer");
                    mesh.Texture = TDContentManager.LoadTexture("ColorPaletteTexture");
                    hammer.Mesh = mesh;
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
            case PrefabType.MenuUIManager:
                {
                    prefab.AddComponent<MenuUIManager>();
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
                    woodObject.RectTransform.LocalPosition = new Vector2(-6f, 10f);
                    stoneObject.RectTransform.Scale = 0.04f * Vector2.One;
                    stoneObject.RectTransform.LocalPosition = new Vector2(-2f + 0.05f * wood.Texture.Width, 10f);
                    foodObject.RectTransform.Scale = 0.08f * Vector2.One;
                    foodObject.RectTransform.LocalPosition = new Vector2(-8f + 0.05f * wood.Texture.Width + 0.06f * stone.Texture.Width, 25f);
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

            case PrefabType.WaveBar:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    WaveBar waveBar = prefab.AddComponent<WaveBar>();
                    background.Texture = TDContentManager.LoadTexture("UIBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * background.Texture.Width, background.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIBar");
                    foreground.Depth = .1f;
                    waveBar.Background = background;
                    waveBar.Foreground = foreground;
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
                    TDSprite playerIconBackground = prefab.AddComponent<TDSprite>();
   
                    PlayerDisplay playerDisplay = prefab.AddComponent<PlayerDisplay>();
                    playerIconBackground.Texture = TDContentManager.LoadTexture("UIPlayer");
                    playerIconBackground.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * playerIconBackground.Texture.Width, playerIconBackground.Texture.Height);

                    TDObject healthBarObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    HealthBar healthBar = healthBarObject.AddComponent<HealthBar>();
                    TDSprite background = healthBarObject.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    healthBarObject.RectTransform.LocalPosition = new Vector2(.5f * playerIconBackground.Texture.Width, -.8f * playerIconBackground.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, healthBarObject.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    foreground.Depth = .9f;
                    healthBar.Background = background;
                    healthBar.Foreground = foreground;
                    healthBar.AlwaysShow = true;
                    playerDisplay.HealthBar = healthBar;

                    TDObject playerIconObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite playerIcon = playerIconObject.AddComponent<TDSprite>();
                    playerIcon.Texture = TDContentManager.LoadTexture("UICinderella");
                    playerIcon.Depth = 0.9f;
                    playerIconObject.RectTransform.Origin = new Vector2(.5f * playerIcon.Texture.Width, playerIcon.Texture.Height);
                    playerIconObject.RectTransform.Scale = 0.3f * Vector2.One;
                    playerIconObject.RectTransform.LocalPosition = new Vector2(0f, 0.7f * playerIconBackground.Texture.Height);
                    playerDisplay.PlayerIcon = playerIcon;

                    TDObject playerNameObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText playerName = playerNameObject.AddComponent<TDText>();
                    playerName.Text = "Player Name";
                    playerName.Depth = 0.9f;
                    playerNameObject.RectTransform.Origin = new Vector2(0.5f * playerName.Width, playerName.Height);
                    playerNameObject.RectTransform.LocalPosition = new Vector2(0f, -2f - playerIconBackground.Texture.Height);
                    playerDisplay.PlayerName = playerName;

                    TDObject currentItemObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite currentItem = currentItemObject.AddComponent<TDSprite>();
                    currentItem.Texture = TDContentManager.LoadTexture("UIAxe");
                    currentItemObject.RectTransform.LocalPosition = new Vector2(.6f * playerIconBackground.Texture.Width, -0.5f * playerIconBackground.Texture.Height);
                    currentItemObject.RectTransform.Scale = 0.15f * Vector2.One;
                    playerDisplay.CurrentItem = currentItem;

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
                    icon.Depth = .9f;
                    iconObject.RectTransform.Origin = .5f * new Vector2(icon.Texture.Width, icon.Texture.Height);
                    iconObject.RectTransform.LocalPosition = -prefab.RectTransform.Origin + new Vector2(60, .5f * buildSprite.Texture.Height + 10);
                    buildMenu.BuildSprite = buildSprite;
                    buildMenu.Icon = icon;

                    TDObject detailObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    detailObject.RectTransform.LocalPosition = new Vector2(icon.Texture.Width, -buildSprite.Texture.Height);

                    TDObject textTitleObject = CreatePrefab(PrefabType.EmptyUI, detailObject.Transform);
                    TDText textTitle = textTitleObject.AddComponent<TDText>();
                    textTitleObject.RectTransform.LocalPosition = new Vector2(0, 30);
                    textTitle.Color = Color.Black;

                    TDObject costObject = CreatePrefab(PrefabType.EmptyUI, detailObject.Transform);
                    costObject.RectTransform.LocalPosition = new Vector2(icon.Texture.Width - 15, 50);

                    int iconWidth = 30;

                    // wood cost
                    TDObject woodCostObject = CreatePrefab(PrefabType.EmptyUI, costObject.Transform);
                    woodCostObject.RectTransform.LocalPosition = Vector2.Zero;

                    TDObject woodObject = CreatePrefab(PrefabType.EmptyUI, woodCostObject.Transform);
                    TDSprite wood = woodObject.AddComponent<TDSprite>();
                    wood.Texture = TDContentManager.LoadTexture("UIWood");
                    woodObject.RectTransform.Scale = iconWidth / (float)wood.Texture.Width * Vector2.One;

                    TDObject textCostWoodObject = CreatePrefab(PrefabType.EmptyUI, woodCostObject.Transform);
                    TDText textCostWood = textCostWoodObject.AddComponent<TDText>();
                    textCostWoodObject.RectTransform.LocalPosition = new Vector2(iconWidth + 5, 0);
                    textCostWood.Color = Color.Black;

                    // stone cost
                    TDObject stoneCostObject = CreatePrefab(PrefabType.EmptyUI, costObject.Transform);
                    stoneCostObject.RectTransform.LocalPosition = new Vector2(0, 15);

                    TDObject stoneObject = CreatePrefab(PrefabType.EmptyUI, stoneCostObject.Transform);
                    TDSprite stone = stoneObject.AddComponent<TDSprite>();
                    stone.Texture = TDContentManager.LoadTexture("UIStone");
                    stoneObject.RectTransform.Scale = iconWidth / (float)stone.Texture.Width * Vector2.One;
                    stoneObject.RectTransform.LocalPosition = new Vector2(0, 0);

                    TDObject textCostStoneObject = CreatePrefab(PrefabType.EmptyUI, stoneCostObject.Transform);
                    TDText textCostStone = textCostStoneObject.AddComponent<TDText>();
                    textCostStoneObject.RectTransform.LocalPosition = new Vector2(iconWidth + 5, 0);
                    textCostStone.Color = Color.Black;

                    // food cost
                    TDObject foodCostObject = CreatePrefab(PrefabType.EmptyUI, costObject.Transform);
                    foodCostObject.RectTransform.LocalPosition = new Vector2(0, 30);

                    TDObject foodObject = CreatePrefab(PrefabType.EmptyUI, foodCostObject.Transform);
                    TDSprite food = foodObject.AddComponent<TDSprite>();
                    food.Texture = TDContentManager.LoadTexture("UIFood");
                    foodObject.RectTransform.Scale = iconWidth / (float)(food.Texture.Width + 100) * Vector2.One;
                    foodObject.RectTransform.LocalPosition = new Vector2(3, 5);

                    TDObject textCostFoodObject = CreatePrefab(PrefabType.EmptyUI, foodCostObject.Transform);
                    TDText textCostFood = textCostFoodObject.AddComponent<TDText>();
                    textCostFoodObject.RectTransform.LocalPosition = new Vector2(iconWidth + 5, 0);
                    textCostFood.Color = Color.Black;

                    buildMenu.Title = textTitle;
                    buildMenu.WoodCost = textCostWood;
                    buildMenu.StoneCost = textCostStone;
                    buildMenu.FoodCost = textCostFood;

                    buildMenu.UiElements.Add(buildSprite);
                    buildMenu.UiElements.Add(icon);
                    buildMenu.UiElements.Add(textTitle);
                    buildMenu.UiElements.Add(textCostFood);
                    buildMenu.UiElements.Add(textCostStone);
                    buildMenu.UiElements.Add(textCostWood);
                    buildMenu.UiElements.Add(food);
                    buildMenu.UiElements.Add(stone);
                    buildMenu.UiElements.Add(wood);

                    break;
                }

            case PrefabType.WaveIndicator:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    WaveIndicator waveIndicator = prefab.AddComponent<WaveIndicator>();
                    WaveBar waveCountDown = prefab.AddComponent<WaveBar>();
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    background.Color = Color.Black;
                    background.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(background.Texture.Width, 0f);
                    prefab.RectTransform.LocalPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 20f, 20f);
                    prefab.RectTransform.Scale = Vector2.One;

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    foreground.Depth = 0.1f;
                    foregroundObject.RectTransform.LocalPosition = new Vector2(-background.Texture.Width, 0f);
                    waveCountDown.Background = background;
                    waveCountDown.Foreground = foreground;
                    waveCountDown.AlwaysShow = true;
                    waveIndicator.WaveCountDown = waveCountDown;

                    TDObject textObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText text = textObject.AddComponent<TDText>();
                    text.Text = "Next Wave:";
                    textObject.RectTransform.LocalPosition = new Vector2(- text.Width - background.Texture.Width - 10f, 0f);
                    waveIndicator.Text = text;

                    TDObject imageObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite image = imageObject.AddComponent<TDSprite>();
                    image.Texture = TDContentManager.LoadTexture("UIWarning");
                    image.Depth = 2f;
                    waveIndicator.Image = image;
                    imageObject.RectTransform.Origin = new Vector2(image.Texture.Width, 0f);
                    imageObject.RectTransform.Scale = 0.2f * Vector2.One;
                    break;
                }

            case PrefabType.GameOverOverlay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDSprite blackOverlay = prefab.AddComponent<TDSprite>();
                    blackOverlay.Texture = TDContentManager.LoadTexture("UIOverlayTexture");
                    blackOverlay.Color = new Color(0f, 0f, 0f, .5f);
                    blackOverlay.Depth = 1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * blackOverlay.Texture.Width, .5f * blackOverlay.Texture.Height);
                    prefab.RectTransform.Position = prefab.RectTransform.Origin;
                    prefab.RectTransform.Scale = 2f * Vector2.One;

                    TDObject gameOverTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText gameOverText = gameOverTextObject.AddComponent<TDText>();
                    gameOverText.Text = "Game Over";
                    gameOverTextObject.RectTransform.Origin = new Vector2(.5f * gameOverText.Width, .5f * gameOverText.Height);
                    gameOverTextObject.RectTransform.LocalPosition = new Vector2(0, -100);

                    TDObject survivalTimeTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText survivalTimeText = survivalTimeTextObject.AddComponent<TDText>();
                    survivalTimeText.Text = "It's over.";
                    survivalTimeTextObject.RectTransform.Origin = new Vector2(.5f * survivalTimeText.Width, .5f * survivalTimeText.Height);
                    survivalTimeTextObject.RectTransform.LocalPosition = new Vector2(0, -40);
                    survivalTimeTextObject.RectTransform.Scale = Vector2.One;

                    TDObject restartTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText restartText = restartTextObject.AddComponent<TDText>();
                    restartText.Text = "Press A to restart!";
                    restartTextObject.RectTransform.Origin = new Vector2(.5f * restartText.Width, .5f * restartText.Height);
                    restartTextObject.RectTransform.LocalPosition = new Vector2(0, 40);
                    restartTextObject.RectTransform.Scale = Vector2.One;

                    GameOverOverlay gameOverOverlay = prefab.AddComponent<GameOverOverlay>();
                    gameOverOverlay.BlackOverlay = blackOverlay;
                    gameOverOverlay.GameOverText = gameOverText;
                    gameOverOverlay.SurvivalTimeText = survivalTimeText;
                    gameOverOverlay.RestartText = restartText;
                    break;
                }

            case PrefabType.MainMenu:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDObject gameLogoObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite gameLogo = gameLogoObject.AddComponent<TDSprite>();
                    gameLogo.Texture = TDContentManager.LoadTexture("UIGameLogo");
                    gameLogo.Depth = 1f;
                    gameLogoObject.RectTransform.Origin = new Vector2(.5f * gameLogo.Texture.Width, .5f * gameLogo.Texture.Height);
                    gameLogoObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight - 600);

                    TDObject startButtonObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite startButton = startButtonObject.AddComponent<TDSprite>();
                    startButton.Texture = TDContentManager.LoadTexture("UIButtonStart");
                    startButton.Depth = 1f;
                    startButtonObject.RectTransform.Origin = new Vector2(.5f * startButton.Texture.Width, .5f * startButton.Texture.Height);
                    startButtonObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight + 200);

                    MainMenu mainMenu = prefab.AddComponent<MainMenu>();
                    mainMenu.GameLogo = gameLogo;

                    break;
                }

            case PrefabType.CharacterDisplay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDObject infoTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText infoText = infoTextObject.AddComponent<TDText>();
                    infoText.Text = "Press    ";
                    infoText.Depth = 1f;
                    infoTextObject.RectTransform.Origin = new Vector2(.5f * infoText.Width, .5f * infoText.Height);
                    infoTextObject.RectTransform.LocalPosition = 200f * Vector2.One;

                    TDObject buttonIconObject = CreatePrefab(PrefabType.EmptyUI, infoTextObject.Transform);
                    TDSprite buttonIcon = buttonIconObject.AddComponent<TDSprite>();
                    buttonIcon.Texture = TDContentManager.LoadTexture("UIXboxA");
                    buttonIconObject.RectTransform.Origin = .5f * new Vector2(buttonIcon.Texture.Width, buttonIcon.Texture.Height);
                    buttonIconObject.RectTransform.LocalPosition = new Vector2(50f, 0f);
                    buttonIconObject.RectTransform.LocalScale = .5f * Vector2.One;

                    TDObject leftArrowObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite leftArrow = leftArrowObject.AddComponent<TDSprite>();
                    leftArrow.Texture = TDContentManager.LoadTexture("UIXboxDpadLeft");
                    leftArrowObject.RectTransform.Origin = .5f * new Vector2(leftArrow.Texture.Width, leftArrow.Texture.Height);
                    leftArrowObject.RectTransform.LocalPosition = new Vector2(120f, 200f);
                    leftArrowObject.RectTransform.LocalScale = .5f * Vector2.One;

                    TDObject rightArrowObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite rightArrow = rightArrowObject.AddComponent<TDSprite>();
                    rightArrow.Texture = TDContentManager.LoadTexture("UIXboxDpadRight");
                    rightArrowObject.RectTransform.Origin = .5f * new Vector2(rightArrow.Texture.Width, rightArrow.Texture.Height);
                    rightArrowObject.RectTransform.LocalPosition = new Vector2(320f, 200f);
                    rightArrowObject.RectTransform.LocalScale = .5f * Vector2.One;

                    CharacterSelectionDisplay characterDisplay = prefab.AddComponent<CharacterSelectionDisplay>();
                    characterDisplay.InfoText = infoText;
                    characterDisplay.ButtonIcon = buttonIcon;
                    characterDisplay.LeftArrow = leftArrow;
                    characterDisplay.RightArrow = rightArrow;

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
        enemy.Collider = collider;
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

    private static void CreateEmptyUI(TDObject prefab, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
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

    private static void CreateEmptyUI3D(TDObject prefab, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        prefab.RectTransform = new TDRectTransform
        {
            TDObject = prefab,
            Parent3D = prefab.Transform.Parent
        };
    }
}
