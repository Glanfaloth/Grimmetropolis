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
    SpeechBubble,
    ButtonIcon,

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
                    TutorialAdvisor tutorialGuy = prefab.AddComponent<TutorialAdvisor>();
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

                    prefab.AddComponent<TutorialPipeline>();

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

                    TDObject woodDisplayObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite woodBackground = woodDisplayObject.AddComponent<TDSprite>();
                    woodBackground.Texture = TDContentManager.LoadTexture("UISquare");
                    woodBackground.Depth = .6f;
                    woodDisplayObject.RectTransform.Origin = .5f * new Vector2(woodBackground.Texture.Width, woodBackground.Texture.Height);
                    woodDisplayObject.RectTransform.Position = woodDisplayObject.RectTransform.Origin;
                    woodDisplayObject.RectTransform.Scale = .75f * Vector2.One;

                    TDSprite woodIcon = CreatePrefab(PrefabType.EmptyUI, woodDisplayObject.Transform).AddComponent<TDSprite>();
                    woodIcon.Texture = TDContentManager.LoadTexture("UIWood");
                    woodIcon.Depth = .55f;
                    woodIcon.TDObject.RectTransform.Origin = .5f * new Vector2(woodIcon.Texture.Width, woodIcon.Texture.Height);
                    woodIcon.TDObject.RectTransform.LocalPosition = new Vector2(6f, 0f);
                    woodIcon.TDObject.RectTransform.LocalScale = .12f * Vector2.One;

                    TDSprite woodTextBackground = CreatePrefab(PrefabType.EmptyUI, woodDisplayObject.Transform).AddComponent<TDSprite>();
                    woodTextBackground.Texture = TDContentManager.LoadTexture("UIRectangle");
                    woodTextBackground.Depth = .65f;
                    woodTextBackground.TDObject.RectTransform.Origin = .5f * new Vector2(0f, woodTextBackground.Texture.Height);
                    woodTextBackground.TDObject.RectTransform.LocalScale = .65f * Vector2.One;

                    TDText woodText = CreatePrefab(PrefabType.EmptyUI, woodDisplayObject.Transform).AddComponent<TDText>();
                    woodText.Text = "0";
                    woodText.Depth = .55f;
                    woodText.TDObject.RectTransform.Origin = .5f * new Vector2(woodText.Width, woodText.Height);
                    woodText.TDObject.RectTransform.LocalPosition = new Vector2(130f, -5f);
                    woodText.TDObject.RectTransform.LocalScale = .75f * Vector2.One;

                    TDObject stoneDisplayObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite stoneBackground = stoneDisplayObject.AddComponent<TDSprite>();
                    stoneBackground.Texture = TDContentManager.LoadTexture("UISquare");
                    stoneBackground.Depth = .6f;
                    stoneDisplayObject.RectTransform.Origin = .5f * new Vector2(stoneBackground.Texture.Width, stoneBackground.Texture.Height);
                    stoneDisplayObject.RectTransform.Position = new Vector2(250f, 0f) + stoneDisplayObject.RectTransform.Origin;
                    stoneDisplayObject.RectTransform.Scale = .75f * Vector2.One;

                    TDSprite stoneIcon = CreatePrefab(PrefabType.EmptyUI, stoneDisplayObject.Transform).AddComponent<TDSprite>();
                    stoneIcon.Texture = TDContentManager.LoadTexture("UIStone");
                    stoneIcon.Depth = .55f;
                    stoneIcon.TDObject.RectTransform.Origin = .5f * new Vector2(stoneIcon.Texture.Width, stoneIcon.Texture.Height);
                    stoneIcon.TDObject.RectTransform.LocalPosition = new Vector2(6f, -2f);
                    stoneIcon.TDObject.RectTransform.LocalScale = .1f * Vector2.One;

                    TDSprite stoneTextBackground = CreatePrefab(PrefabType.EmptyUI, stoneDisplayObject.Transform).AddComponent<TDSprite>();
                    stoneTextBackground.Texture = TDContentManager.LoadTexture("UIRectangle");
                    stoneTextBackground.Depth = .65f;
                    stoneTextBackground.TDObject.RectTransform.Origin = .5f * new Vector2(0f, woodTextBackground.Texture.Height);
                    stoneTextBackground.TDObject.RectTransform.LocalScale = .65f * Vector2.One;

                    TDText stoneText = CreatePrefab(PrefabType.EmptyUI, stoneDisplayObject.Transform).AddComponent<TDText>();
                    stoneText.Text = "0";
                    stoneText.Depth = .55f;
                    stoneText.TDObject.RectTransform.Origin = .5f * new Vector2(stoneText.Width, stoneText.Height);
                    stoneText.TDObject.RectTransform.LocalPosition = new Vector2(130f, -5f);
                    stoneText.TDObject.RectTransform.LocalScale = .75f * Vector2.One;

                    TDObject foodDisplayObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foodBackground = foodDisplayObject.AddComponent<TDSprite>();
                    foodBackground.Texture = TDContentManager.LoadTexture("UISquare");
                    foodBackground.Depth = .6f;
                    foodDisplayObject.RectTransform.Origin = .5f * new Vector2(foodBackground.Texture.Width, foodBackground.Texture.Height);
                    foodDisplayObject.RectTransform.Position = new Vector2(500f, 0f) + foodDisplayObject.RectTransform.Origin;
                    foodDisplayObject.RectTransform.Scale = .75f * Vector2.One;

                    TDSprite foodIcon = CreatePrefab(PrefabType.EmptyUI, foodDisplayObject.Transform).AddComponent<TDSprite>();
                    foodIcon.Texture = TDContentManager.LoadTexture("UIFood");
                    foodIcon.Depth = .55f;
                    foodIcon.TDObject.RectTransform.Origin = .5f * new Vector2(foodIcon.Texture.Width, foodIcon.Texture.Height);
                    foodIcon.TDObject.RectTransform.LocalPosition = new Vector2(6f, 0f);
                    foodIcon.TDObject.RectTransform.LocalScale = .2f * Vector2.One;

                    TDSprite foodTextBackground = CreatePrefab(PrefabType.EmptyUI, foodDisplayObject.Transform).AddComponent<TDSprite>();
                    foodTextBackground.Texture = TDContentManager.LoadTexture("UIRectangle");
                    foodTextBackground.Depth = .65f;
                    foodTextBackground.TDObject.RectTransform.Origin = .5f * new Vector2(0f, woodTextBackground.Texture.Height);
                    foodTextBackground.TDObject.RectTransform.LocalScale = .65f * Vector2.One;

                    TDText foodText = CreatePrefab(PrefabType.EmptyUI, foodDisplayObject.Transform).AddComponent<TDText>();
                    foodText.Text = "0";
                    foodText.Depth = .55f;
                    foodText.TDObject.RectTransform.Origin = .5f * new Vector2(foodText.Width, foodText.Height);
                    foodText.TDObject.RectTransform.LocalPosition = new Vector2(130f, -5f);
                    foodText.TDObject.RectTransform.LocalScale = .75f * Vector2.One;

                    ResourceDisplay resourceDisplay = prefab.AddComponent<ResourceDisplay>();
                    resourceDisplay.WoodText = woodText;
                    resourceDisplay.StoneText = stoneText;
                    resourceDisplay.FoodText = foodText;
                    break;
                }

            case PrefabType.HealthBar:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    HealthBar healthBar = prefab.AddComponent<HealthBar>();
                    background.Texture = TDContentManager.LoadTexture("UIBar");
                    background.Color = Color.Black;
                    background.Depth = .9f;
                    prefab.RectTransform.Origin = new Vector2(.5f * background.Texture.Width, background.Texture.Height);
                    prefab.RectTransform.LocalScale = new Vector2(1f, .5f);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIBar");
                    foreground.Depth = .8f;
                    healthBar.Background = background;
                    healthBar.Foreground = foreground;
                    foregroundObject.RectTransform.LocalPosition = -prefab.RectTransform.Origin;
                    prefab.RectTransform.LocalScale = new Vector2(1f, .5f);
                    break;
                }

            case PrefabType.WaveBar:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);
                    TDSprite background = prefab.AddComponent<TDSprite>();
                    WaveBar waveBar = prefab.AddComponent<WaveBar>();
                    background.Texture = TDContentManager.LoadTexture("UIBar");
                    background.Color = Color.Black;
                    background.Depth = .2f;
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
                    background.Depth = .9f;
                    prefab.RectTransform.Origin = new Vector2(.5f * background.Texture.Width, background.Texture.Height);
                    prefab.RectTransform.LocalScale = new Vector2(1f, .5f);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIBar");
                    foreground.Depth = .8f;
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
                    playerIconBackground.Texture = TDContentManager.LoadTexture("UISquare");
                    playerIconBackground.Depth = .7f;
                    prefab.RectTransform.Origin = new Vector2(.5f * playerIconBackground.Texture.Width, playerIconBackground.Texture.Height);

                    TDObject healthBarObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    HealthBar healthBar = healthBarObject.AddComponent<HealthBar>();
                    TDSprite background = healthBarObject.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    background.Color = Color.Black;
                    background.Depth = .6f;
                    healthBarObject.RectTransform.LocalPosition = new Vector2(.5f * playerIconBackground.Texture.Width, -.8f * playerIconBackground.Texture.Height);

                    TDObject foregroundObject = CreatePrefab(PrefabType.EmptyUI, healthBarObject.Transform);
                    TDSprite foreground = foregroundObject.AddComponent<TDSprite>();
                    foreground.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    foreground.Depth = .5f;
                    healthBar.Background = background;
                    healthBar.Foreground = foreground;
                    healthBar.AlwaysShow = true;
                    playerDisplay.HealthBar = healthBar;

                    TDObject playerIconObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite playerIcon = playerIconObject.AddComponent<TDSprite>();
                    playerIcon.Texture = TDContentManager.LoadTexture("UICinderella");
                    playerIcon.Depth = .4f;
                    playerIconObject.RectTransform.Origin = new Vector2(.5f * playerIcon.Texture.Width, playerIcon.Texture.Height);
                    playerIconObject.RectTransform.Scale = 0.3f * Vector2.One;
                    playerIconObject.RectTransform.LocalPosition = new Vector2(.05f * playerIconBackground.Texture.Width, 0.625f * playerIconBackground.Texture.Height);
                    playerDisplay.PlayerIcon = playerIcon;

                    TDObject playerNameObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText playerName = playerNameObject.AddComponent<TDText>();
                    playerName.Text = "Player Name";
                    playerName.Depth = 0.5f;
                    playerNameObject.RectTransform.Origin = new Vector2(0.5f * playerName.Width, playerName.Height);
                    playerNameObject.RectTransform.LocalPosition = new Vector2(20f, -2f - playerIconBackground.Texture.Height);
                    playerNameObject.RectTransform.LocalScale = .5f * Vector2.One;
                    playerDisplay.PlayerName = playerName;

                    TDObject currentItemObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite currentItem = currentItemObject.AddComponent<TDSprite>();
                    currentItem.Texture = TDContentManager.LoadTexture("UIAxe");
                    currentItem.Depth = .4f;
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
                    prefab.RectTransform.Origin = new Vector2(0f, buildSprite.Texture.Height);
                    prefab.RectTransform.Scale = .15f * Vector2.One;

                    TDObject iconObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite icon = iconObject.AddComponent<TDSprite>();
                    icon.Texture = TDContentManager.LoadTexture("UIBuildingOutpostIcon");
                    iconObject.RectTransform.Origin = .5f * new Vector2(icon.Texture.Width, icon.Texture.Height);
                    iconObject.RectTransform.LocalPosition = new Vector2(440f, 440f) - prefab.RectTransform.Origin;
                    iconObject.RectTransform.Scale = Vector2.One;
                    buildMenu.BuildSprite = buildSprite;
                    buildMenu.Icon = icon;

                    TDObject textTitleObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText textTitle = textTitleObject.AddComponent<TDText>();
                    textTitleObject.RectTransform.Origin = .5f * new Vector2(textTitle.Width, textTitle.Height);
                    textTitleObject.RectTransform.LocalPosition = new Vector2(650f, 120f) - prefab.RectTransform.Origin;
                    textTitleObject.RectTransform.Scale = .3f * Vector2.One;

                    // wood cost
                    TDObject woodObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite wood = woodObject.AddComponent<TDSprite>();
                    wood.Texture = TDContentManager.LoadTexture("UIWood");
                    woodObject.RectTransform.Position = new Vector2(85f, -75f);
                    woodObject.RectTransform.Scale = .035f * Vector2.One;

                    TDObject textCostWoodObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText textCostWood = textCostWoodObject.AddComponent<TDText>();
                    textCostWoodObject.RectTransform.Origin = .5f * new Vector2(0f, textCostWood.Width);
                    textCostWoodObject.RectTransform.LocalPosition = new Vector2(905f, -470f);
                    textCostWoodObject.RectTransform.Scale = .3f * Vector2.One;

                    // stone cost
                    TDObject stoneObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite stone = stoneObject.AddComponent<TDSprite>();
                    stone.Texture = TDContentManager.LoadTexture("UIStone");
                    stoneObject.RectTransform.Position = new Vector2(90f, -50f);
                    stoneObject.RectTransform.Scale = .03f * Vector2.One;

                    TDObject textCostStoneObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText textCostStone = textCostStoneObject.AddComponent<TDText>();
                    textCostStoneObject.RectTransform.Origin = .5f * new Vector2(0f, textCostStone.Width);
                    textCostStoneObject.RectTransform.LocalPosition = new Vector2(905f, -300f);
                    textCostStoneObject.RectTransform.Scale = .3f * Vector2.One;

                    buildMenu.Title = textTitle;
                    buildMenu.WoodCost = textCostWood;
                    buildMenu.StoneCost = textCostStone;

                    float elementDepth = .6f;
                    buildSprite.Depth = .7f;
                    icon.Depth = elementDepth;
                    textTitle.Depth = elementDepth;
                    textCostStone.Depth = elementDepth;
                    textCostWood.Depth = elementDepth;
                    stone.Depth = elementDepth;
                    wood.Depth = elementDepth;

                    buildMenu.UiElements.Add(buildSprite);
                    buildMenu.UiElements.Add(icon);
                    buildMenu.UiElements.Add(textTitle);
                    buildMenu.UiElements.Add(textCostStone);
                    buildMenu.UiElements.Add(textCostWood);
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
                    background.Depth = .4f;
                    prefab.RectTransform.Origin = new Vector2(background.Texture.Width, 0f);
                    prefab.RectTransform.LocalPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 30f, 30f);

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

                    TDObject warningSignObject = CreatePrefab(PrefabType.EmptyUI3D, prefab.Transform);
                    TDSprite warningSign = warningSignObject.AddComponent<TDSprite>();
                    warningSign.Texture = TDContentManager.LoadTexture("UIWarning");
                    warningSign.Depth = .5f;
                    waveIndicator.WarningSign = warningSign;
                    warningSignObject.RectTransform.Origin = new Vector2(.5f * warningSign.Texture.Width, warningSign.Texture.Height);
                    warningSignObject.RectTransform.Offset = 0.2f * Vector3.Backward;
                    warningSignObject.RectTransform.Scale = 0.15f * Vector2.One;
                    break;
                }

            case PrefabType.GameOverOverlay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDSprite blackOverlay = prefab.AddComponent<TDSprite>();
                    blackOverlay.Texture = TDContentManager.LoadTexture("UIOverlayTexture");
                    blackOverlay.Color = new Color(0f, 0f, 0f, .5f);
                    blackOverlay.Depth = .1f;
                    prefab.RectTransform.Origin = new Vector2(.5f * blackOverlay.Texture.Width, .5f * blackOverlay.Texture.Height);
                    prefab.RectTransform.Position = prefab.RectTransform.Origin;
                    prefab.RectTransform.Scale = 2f * Vector2.One;

                    TDObject gameOverSpriteObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite gameOverSprite = gameOverSpriteObject.AddComponent<TDSprite>();
                    gameOverSprite.Texture = TDContentManager.LoadTexture("UIGameOver");
                    gameOverSpriteObject.RectTransform.Origin = new Vector2(.5f * gameOverSprite.Texture.Width, .5f * gameOverSprite.Texture.Height);
                    gameOverSpriteObject.RectTransform.LocalPosition = new Vector2(0, -250);
                    prefab.RectTransform.Scale = Vector2.One;

                    TDObject survivalTimeTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText survivalTimeText = survivalTimeTextObject.AddComponent<TDText>();
                    survivalTimeText.Text = "It's over.";
                    survivalTimeTextObject.RectTransform.Origin = new Vector2(.5f * survivalTimeText.Width, .5f * survivalTimeText.Height);
                    survivalTimeTextObject.RectTransform.LocalPosition = new Vector2(0, -40);
                    survivalTimeTextObject.RectTransform.Scale = Vector2.One;

                    TDObject restartTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText restartText = restartTextObject.AddComponent<TDText>();
                    restartText.Text = "Press RB to restart!";
                    restartTextObject.RectTransform.Origin = new Vector2(.5f * restartText.Width, .5f * restartText.Height);
                    restartTextObject.RectTransform.LocalPosition = new Vector2(0, 40);
                    restartTextObject.RectTransform.Scale = Vector2.One;

                    GameOverOverlay gameOverOverlay = prefab.AddComponent<GameOverOverlay>();
                    gameOverOverlay.BlackOverlay = blackOverlay;
                    gameOverOverlay.GameOverText = gameOverSprite;
                    gameOverOverlay.SurvivalTimeText = survivalTimeText;
                    gameOverOverlay.RestartText = restartText;
                    break;
                }

            case PrefabType.MainMenu:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);

                    TDObject splashScreenObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite splashScreen = splashScreenObject.AddComponent<TDSprite>();
                    splashScreen.Texture = TDContentManager.LoadTexture("SplashScreen");
                    splashScreen.Depth = .2f;

                    TDObject splashScreenTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText splashScreenText = splashScreenTextObject.AddComponent<TDText>();
                    splashScreenText.Text = "Press   ";
                    splashScreenText.Depth = .1f;
                    splashScreenTextObject.RectTransform.Origin = new Vector2(splashScreenText.Width, splashScreenText.Height);
                    splashScreenTextObject.RectTransform.Position = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 150f, TDSceneManager.Graphics.PreferredBackBufferHeight - 50f);

                    TDObject splashScreenButtonObject = CreatePrefab(PrefabType.EmptyUI, splashScreenTextObject.Transform);
                    TDSprite splashScreenButton = splashScreenButtonObject.AddComponent<TDSprite>();
                    splashScreenButton.Texture = TDContentManager.LoadTexture("UIXboxA");
                    splashScreenButton.Depth = .1f;
                    splashScreenButtonObject.RectTransform.Origin = new Vector2(splashScreenButton.Texture.Width, splashScreenButton.Texture.Height);
                    splashScreenButtonObject.RectTransform.Scale = .75f * Vector2.One;
                    splashScreenButtonObject.RectTransform.LocalPosition = new Vector2(40f, 0f);

                    TDObject gameLogoObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite gameLogo = gameLogoObject.AddComponent<TDSprite>();
                    gameLogo.Texture = TDContentManager.LoadTexture("UIGameLogo");
                    gameLogo.Depth = .9f;
                    gameLogoObject.RectTransform.Origin = new Vector2(.5f * gameLogo.Texture.Width, .5f * gameLogo.Texture.Height);
                    gameLogoObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight - 600);

                    TDObject startButtonObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite startButton = startButtonObject.AddComponent<TDSprite>();
                    startButton.Texture = TDContentManager.LoadTexture("UIButtonStart");
                    startButton.Depth = .9f;
                    startButtonObject.RectTransform.Origin = new Vector2(.5f * startButton.Texture.Width, .5f * startButton.Texture.Height + 200f);
                    startButtonObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight + 200);

                    TDObject settingsButtonObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite settingsButton = settingsButtonObject.AddComponent<TDSprite>();
                    settingsButton.Texture = TDContentManager.LoadTexture("UIButtonSettings");
                    settingsButton.Depth = .9f;
                    settingsButtonObject.RectTransform.Origin = new Vector2(.5f * settingsButton.Texture.Width, .5f * settingsButton.Texture.Height);
                    settingsButtonObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight + 200);

                    TDObject soundIconObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite soundIcon = soundIconObject.AddComponent<TDSprite>();
                    soundIcon.Texture = TDContentManager.LoadTexture("UIVolumeOn");
                    soundIcon.Depth = .9f;
                    soundIconObject.RectTransform.Origin = new Vector2(.5f * soundIcon.Texture.Width, .5f * soundIcon.Texture.Height);
                    soundIconObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 400f, TDSceneManager.Graphics.PreferredBackBufferHeight);
                    soundIconObject.RectTransform.Scale = .6f * Vector2.One;

                    TDObject soundBarBackgroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite soundBarBackground = soundBarBackgroundObject.AddComponent<TDSprite>();
                    soundBarBackground.Texture = TDContentManager.LoadTexture("UISoundBarBack");
                    soundBarBackground.Depth = .95f;
                    soundBarBackgroundObject.RectTransform.Origin = new Vector2(0f, .5f * soundBarBackground.Texture.Height);
                    soundBarBackgroundObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 200f, TDSceneManager.Graphics.PreferredBackBufferHeight);

                    TDObject soundBarForegroundObject = CreatePrefab(PrefabType.EmptyUI, soundBarBackgroundObject.Transform);
                    TDSprite soundBarForeground = soundBarForegroundObject.AddComponent<TDSprite>();
                    soundBarForeground.Texture = TDContentManager.LoadTexture("UISoundBarFront");
                    soundBarForeground.Depth = .85f;
                    soundBarForegroundObject.RectTransform.Origin = new Vector2(0f, .5f * soundBarForeground.Texture.Height);

                    TDObject soundBarObject = CreatePrefab(PrefabType.EmptyUI, soundBarBackgroundObject.Transform);
                    TDSprite soundBar = soundBarObject.AddComponent<TDSprite>();
                    soundBar.Texture = TDContentManager.LoadTexture("UIPlayerBar");
                    soundBar.Color = Color.Green;
                    soundBar.Depth = .9f;
                    soundBarObject.RectTransform.Origin = new Vector2(0f, .5f * soundBar.Texture.Height);
                    soundBarObject.RectTransform.LocalPosition = new Vector2(10f, -2f);
                    soundBarObject.RectTransform.Scale = new Vector2(2.81f, 1.4f);

                    TDObject goBackTextObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDText goBackText = goBackTextObject.AddComponent<TDText>();
                    goBackText.Text = "Back   ";
                    goBackText.Depth = .9f;
                    goBackTextObject.RectTransform.Origin = .5f * new Vector2(goBackText.Width, goBackText.Height);
                    goBackTextObject.RectTransform.Position = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight + 300f);
                    goBackTextObject.RectTransform.Scale = .5f * Vector2.One;

                    TDObject goBackButtonObject = CreatePrefab(PrefabType.EmptyUI, goBackTextObject.Transform);
                    TDSprite goBackButton = goBackButtonObject.AddComponent<TDSprite>();
                    goBackButton.Depth = .9f;
                    goBackButton.Texture = TDContentManager.LoadTexture("UIXBoxB");
                    goBackButtonObject.RectTransform.Origin = .5f * new Vector2(goBackButton.Texture.Width, goBackButton.Texture.Height);
                    goBackButtonObject.RectTransform.LocalPosition = new Vector2(120f, 0f);


                    MainMenu mainMenu = prefab.AddComponent<MainMenu>();
                    mainMenu.SplashScreen = splashScreen;
                    mainMenu.SplashScreenText = splashScreenText;
                    mainMenu.SplashScreenButton = splashScreenButton;
                    mainMenu.GameLogo = gameLogo;

                    mainMenu.StartButton = startButton;
                    mainMenu.SettingsButton = settingsButton;
                    mainMenu.SoundIcon = soundIcon;
                    mainMenu.SoundBarBack = soundBarBackground;
                    mainMenu.SoundBarFront = soundBarForeground;
                    mainMenu.SoundBar = soundBar;

                    mainMenu.GoBackButton = goBackButton;
                    mainMenu.GoBackText = goBackText;
                    break;
                }

            case PrefabType.CharacterDisplay:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);

                    TDObject backgroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite background = backgroundObject.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UIBoard");
                    background.Depth = .9f;
                    backgroundObject.RectTransform.LocalScale = .4f * Vector2.One;

                    TDObject infoTextObject = CreatePrefab(PrefabType.EmptyUI, backgroundObject.Transform);
                    TDText infoText = infoTextObject.AddComponent<TDText>();
                    infoText.Text = "Press   ";
                    infoText.Depth = .8f;
                    infoTextObject.RectTransform.Origin = new Vector2(.5f * infoText.Width, .5f * infoText.Height);
                    infoTextObject.RectTransform.LocalPosition = new Vector2(.5f * background.Texture.Width - 5f, 55f);
                    infoTextObject.RectTransform.Scale = .35f * Vector2.One;

                    TDObject buttonIconObject = CreatePrefab(PrefabType.EmptyUI, infoTextObject.Transform);
                    TDSprite buttonIcon = buttonIconObject.AddComponent<TDSprite>();
                    buttonIcon.Texture = TDContentManager.LoadTexture("UIXboxA");
                    buttonIcon.Depth = .8f;
                    buttonIconObject.RectTransform.Origin = .5f * new Vector2(buttonIcon.Texture.Width, buttonIcon.Texture.Height);
                    buttonIconObject.RectTransform.LocalPosition = new Vector2(105f, 5f);
                    buttonIconObject.RectTransform.Scale = .3f * Vector2.One;

                    TDObject leftArrowObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite leftArrow = leftArrowObject.AddComponent<TDSprite>();
                    leftArrow.Texture = TDContentManager.LoadTexture("UIButtonLeft");
                    leftArrow.Depth = .8f;
                    leftArrowObject.RectTransform.Origin = .5f * new Vector2(leftArrow.Texture.Width, leftArrow.Texture.Height);
                    leftArrowObject.RectTransform.LocalPosition = new Vector2(115f, 180f);
                    leftArrowObject.RectTransform.LocalScale = .5f * Vector2.One;

                    TDObject rightArrowObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite rightArrow = rightArrowObject.AddComponent<TDSprite>();
                    rightArrow.Texture = TDContentManager.LoadTexture("UIButtonRight");
                    rightArrow.Depth = .8f;
                    rightArrowObject.RectTransform.Origin = .5f * new Vector2(rightArrow.Texture.Width, rightArrow.Texture.Height);
                    rightArrowObject.RectTransform.LocalPosition = new Vector2(305f, 180f);
                    rightArrowObject.RectTransform.LocalScale = .5f * Vector2.One;

                    CharacterSelectionDisplay characterDisplay = prefab.AddComponent<CharacterSelectionDisplay>();
                    characterDisplay.InfoText = infoText;
                    characterDisplay.ButtonIcon = buttonIcon;
                    characterDisplay.LeftArrow = leftArrow;
                    characterDisplay.RightArrow = rightArrow;

                    break;
                }

            case PrefabType.SpeechBubble:
                {
                    CreateEmptyUI(prefab, localPosition, localRotation, localScale);
                    TDObject backgroundObject = CreatePrefab(PrefabType.EmptyUI, prefab.Transform);
                    TDSprite background = backgroundObject.AddComponent<TDSprite>();
                    background.Texture = TDContentManager.LoadTexture("UISpeechBubble");
                    background.Depth = .2f;
                    backgroundObject.RectTransform.Origin = .5f * new Vector2(background.Texture.Width, background.Texture.Height);
                    backgroundObject.RectTransform.LocalPosition = .5f * new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth, TDSceneManager.Graphics.PreferredBackBufferHeight);
                    backgroundObject.RectTransform.LocalScale = .5f * Vector2.One;

                    TDObject speakerIconObject = CreatePrefab(PrefabType.EmptyUI, backgroundObject.Transform);
                    TDSprite speakerIcon = speakerIconObject.AddComponent<TDSprite>();
                    speakerIcon.Texture = TDContentManager.LoadTexture("UITutorialAdvisor");
                    speakerIcon.Depth = .1f;
                    speakerIconObject.RectTransform.Origin = .5f * new Vector2(speakerIcon.Texture.Width, speakerIcon.Texture.Height);
                    speakerIconObject.RectTransform.LocalPosition = new Vector2(190f, 90f) - backgroundObject.RectTransform.Origin;
                    speakerIconObject.RectTransform.LocalScale = .4f * Vector2.One;

                    TDObject messageObject = CreatePrefab(PrefabType.EmptyUI, backgroundObject.Transform);
                    TDText message = messageObject.AddComponent<TDText>();
                    message.Text = "This is an example text. A very long text.\nWith this, I can check how much text fits in one line.\nDo such weird signs work as well?";
                    message.Color = Color.Black;
                    message.Depth = .1f;
                    messageObject.RectTransform.LocalPosition = new Vector2(110f, 200f) - backgroundObject.RectTransform.Origin;
                    messageObject.RectTransform.Scale = .35f * Vector2.One;

                    TDObject buttonIconObject = CreatePrefab(PrefabType.EmptyUI, backgroundObject.Transform);
                    TDSprite buttonIcon = buttonIconObject.AddComponent<TDSprite>();
                    buttonIcon.Texture = TDContentManager.LoadTexture("UIXboxA");
                    buttonIcon.Depth = .1f;
                    buttonIconObject.RectTransform.Origin = .5f * new Vector2(buttonIcon.Texture.Width, buttonIcon.Texture.Height);
                    buttonIconObject.RectTransform.LocalPosition = backgroundObject.RectTransform.Origin - 40f * Vector2.One;
                    buttonIconObject.RectTransform.LocalScale = .5f * Vector2.One;

                    SpeechBubble speechBubble = prefab.AddComponent<SpeechBubble>();
                    speechBubble.Background = background;
                    speechBubble.SpeakerIcon = speakerIcon;
                    speechBubble.Message = message;
                    speechBubble.ButtonIcon = buttonIcon;
                    break;
                }

            case PrefabType.ButtonIcon:
                {
                    CreateEmptyUI3D(prefab, localPosition, localRotation, localScale);

                    TDSprite icon = prefab.AddComponent<TDSprite>();
                    icon.Texture = TDContentManager.LoadTexture("UISpeechBubble");
                    icon.Depth = .9f;

                    ButtonIcon buttonIcon = prefab.AddComponent<ButtonIcon>();
                    buttonIcon.Icon = icon;

                    prefab.RectTransform.Scale = .5f * Vector2.One;

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
    internal static void SpawnPlayer(PlayerInfo playerInfo, Vector3 localPosition)
    {
        Player newPlayer = CreatePrefab(PrefabType.Player, localPosition, Quaternion.Identity, playerInfo.ParentTransform).GetComponent<Player>();
        newPlayer.UiIndex = playerInfo.UIIndex;
        newPlayer.PlayerType = playerInfo.Type;
        if (newPlayer.Animation is CharacterAnimation characterAnimation) characterAnimation.CharacterModel = characterAnimation.GetModelFromPlayerType(playerInfo.Type);
        newPlayer.Input = TDInputManager.PlayerInputs[playerInfo.InputIndex];
        newPlayer.Info = playerInfo;
        playerInfo.Instance = newPlayer;
    }
}
