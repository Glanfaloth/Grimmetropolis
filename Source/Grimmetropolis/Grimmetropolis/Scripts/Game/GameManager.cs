using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Collections.Generic;

public enum GameState
{
    Playing,
    GameOver
}

public class GameManager : TDComponent
{
    public static GameManager Instance;

    public static PlayerType[] PlayerTypes = new PlayerType[4];
    public static List<int> PlayerTypeIndices = new List<int>();

    public Map Map;
    private ResourcePile _resourcePool;
    public ResourcePile ResourcePool
    {
        get => _resourcePool;
        set
        {
            _resourcePool = value;
            UIManager.Instance?.ResourceDisplay.UpdateDisplay();
        }
    }

    private GameState _gameState = GameState.Playing;
    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            if (_gameState == GameState.GameOver) UIManager.Instance.ShowGameOver();
        }
    }

    public EnemyController EnemyController;
    public List<Player> Players = new List<Player>();
    public List<Enemy> Enemies = new List<Enemy>();
    public List<Structure> Structures = new List<Structure>();
    public List<Item> Items = new List<Item>();

    public TDTransform PlayerTransform;
    public TDTransform EnemyTransform;
    public TDTransform StructureTransform;
    public TDTransform ItemTransform;

    private EnemyGroup _spawnGroup;

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        // Map
        TDObject mapObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        Map = mapObject.AddComponent<Map>();
        // TODO: maybe use Content.Load functionalitiy of mgcb
        // WARNING: testEmpty128 is laggy.
        List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/default.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testWitchFlying.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testAvoidOutpost.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testAttackOutpost.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testAttackRangedOutpost.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testEmpty64.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testEmpty128.map");

        // ResourcePool
        ResourcePool = new ResourcePile();

        // EnemyBrain
        TDObject enemyAI = PrefabFactory.CreatePrefab(PrefabType.Empty);
        EnemyController = enemyAI.AddComponent<EnemyController>();
        EnemyController.Configure(Config.TIME_UNTIL_FIRST_WAVE, Config.TIME_BETWEEN_WAVES, Config.FIRST_WAVE_KNIGHT_COUNT, Config.FIRST_WAVE_WITCH_COUNT, Config.FIRST_WAVE_SIEGE_COUNT, Config.WAVE_GROWTH_FACTOR);

        _spawnGroup = new EnemyGroup(null, EnemyController, new MapSpawnDebugState());
        EnemyController.Groups.Add(_spawnGroup);
        // Spawner
        //// TODO: replace test spawner with real spawner
        //TDObject testSpawner = PrefabFactory.CreatePrefab(PrefabType.Empty);
        //var spawner = testSpawner.AddComponent<TestSpawner>();

        // Players
        TDObject playerList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        PlayerTransform = playerList.Transform;

        if (PlayerTypes[0] != PlayerType.None)
        {
            Player player0 = PrefabFactory.CreatePrefab(PrefabType.Player, new Vector3(3, -2, 0), Quaternion.Identity, playerList.Transform).GetComponent<Player>();
            if (player0.Animation is CharacterAnimation characterAnimation) characterAnimation.CharacterModel = characterAnimation.GetModelFromPlayerType(PlayerTypes[0]);
            player0.Input = TDInputManager.PlayerInputs[PlayerTypeIndices[0]];
        }

        if (PlayerTypes[1] != PlayerType.None)
        {
            Player player1 = PrefabFactory.CreatePrefab(PrefabType.Player, new Vector3(3, -1, 0), Quaternion.Identity, playerList.Transform).GetComponent<Player>();
            if (player1.Animation is CharacterAnimation characterAnimation) characterAnimation.CharacterModel = characterAnimation.GetModelFromPlayerType(PlayerTypes[1]);
            player1.Input = TDInputManager.PlayerInputs[PlayerTypeIndices[1]];
        }

        if (PlayerTypes[2] != PlayerType.None)
        {
            Player player2 = PrefabFactory.CreatePrefab(PrefabType.Player, new Vector3(4, -2, 0), Quaternion.Identity, playerList.Transform).GetComponent<Player>();
            if (player2.Animation is CharacterAnimation characterAnimation) characterAnimation.CharacterModel = characterAnimation.GetModelFromPlayerType(PlayerTypes[2]);
            player2.Input = TDInputManager.PlayerInputs[PlayerTypeIndices[2]];
        }
        if (PlayerTypes[3] != PlayerType.None)
        {
            Player player3 = PrefabFactory.CreatePrefab(PrefabType.Player, new Vector3(4, -1, 0), Quaternion.Identity, playerList.Transform).GetComponent<Player>();
            if (player3.Animation is CharacterAnimation characterAnimation) characterAnimation.CharacterModel = characterAnimation.GetModelFromPlayerType(PlayerTypes[3]);
            player3.Input = TDInputManager.PlayerInputs[PlayerTypeIndices[3]];
        }

        // Enemies
        TDObject enemyList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        EnemyTransform = enemyList.Transform;

        // Structures
        TDObject structureList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        StructureTransform = structureList.Transform;

        foreach (var entityToSpawn in entitiesToSpawn)
        {
            SpawnEntity(enemyList, entityToSpawn);
        }

        if (EnemyController.SpawnLocations.Count == 0)
        {
            EnemyController.SpawnLocations.Add(Point.Zero);
        }

        // Items
        TDObject itemList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        ItemTransform = itemList.Transform;

        // magicalArtifact.Character = playerObject1.GetComponent<Player>();

        //TDObject castleObject = PrefabFactory.CreatePrefab(PrefabType.Castle, StructureTransform);
        //Castle castle = castleObject.GetComponent<Castle>();
        //castle.Position = new Point(3, 8);

        //TDObject resourceWoodObject0 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        //ResourceDeposit resourceWood0 = resourceWoodObject0.GetComponent<ResourceDeposit>();
        //resourceWood0.Position = new Point(6, 4);
        //TDObject resourceWoodObject1 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        //ResourceDeposit resourceWood1 = resourceWoodObject1.GetComponent<ResourceDeposit>();
        //resourceWood1.Position = new Point(7, 4);
        //TDObject resourceWoodObject2 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        //ResourceDeposit resourceWood2 = resourceWoodObject2.GetComponent<ResourceDeposit>();
        //resourceWood2.Position = new Point(6, 5);
        //TDObject resourceWoodObject3 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        //ResourceDeposit resourceWood3 = resourceWoodObject3.GetComponent<ResourceDeposit>();
        //resourceWood3.Position = new Point(7, 5);
        //TDObject resourceStoneObject = PrefabFactory.CreatePrefab(PrefabType.Stone, StructureTransform);
        //ResourceDeposit resourceStone = resourceStoneObject.GetComponent<ResourceDeposit>();
        //resourceStone.Position = new Point(7, 10);

        /*TDObject[] testObject = new TDObject[1000];
        TDObject.RunAction(4f, (p) => { }, () =>
        {
            Debug.WriteLine("CreatingObjects");
            for (int i = 0; i < testObject.Length; i++)
            {
                testObject[i] = PrefabFactory.CreatePrefab(PrefabType.Arrow);
            }
        });

        TDObject.RunAction(8f, (p) => { }, () =>
        {
            Debug.WriteLine("DeletingObjects");
            for (int i = 0; i < testObject.Length; i++)
            {
                testObject[i].Destroy();
            }
        });*/
    }

    private void SpawnEntity(TDObject enemyList, MapDTO.EntityToSpawn entityToSpawn)
    {
        switch (entityToSpawn.Type)
        {
            case MapDTO.EntityType.WoodResource:
                {
                    TDObject newEntity = PrefabFactory.CreatePrefab(PrefabType.ResourceWood, StructureTransform);
                    newEntity.GetComponent<ResourceDeposit>().Position = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.StoneResource:
                {
                    TDObject newEntity = PrefabFactory.CreatePrefab(PrefabType.ResourceStone, StructureTransform);
                    newEntity.GetComponent<ResourceDeposit>().Position = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.Castle:
                {
                    // TODO: do we need to check there is only one?
                    TDObject newEntity = PrefabFactory.CreatePrefab(PrefabType.BuildingCastle, StructureTransform);
                    newEntity.GetComponent<Castle>().Position = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.EnemyWitch:
                {
                    var enemy = PrefabFactory.CreateEnemyPrefab<EnemyWitch>(Config.ENEMY_WITCH_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                        Quaternion.Identity, enemyList.Transform);
                    _spawnGroup.Witches.Add(enemy.GetComponent<EnemyWitch>());
                }
                break;
            case MapDTO.EntityType.EnemyKnight:
                {
                    var enemy = PrefabFactory.CreateEnemyPrefab<EnemyKnight>(Config.ENEMY_KNIGHTS_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                        Quaternion.Identity, enemyList.Transform);
                    _spawnGroup.Knights.Add(enemy.GetComponent<EnemyKnight>());
                }
                break;
            case MapDTO.EntityType.EnemyCatapult:
                {
                    var enemy = PrefabFactory.CreateEnemyPrefab<EnemyCatapult>(Config.ENEMY_CATAPULT_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                        Quaternion.Identity, enemyList.Transform);
                    _spawnGroup.Catapults.Add(enemy.GetComponent<EnemyCatapult>());
                }
                break;
            case MapDTO.EntityType.Outpost:
                {
                    TDObject newEntity = PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, StructureTransform);
                    newEntity.GetComponent<Outpost>().Position = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.EnemySpawnPoint:
                EnemyController.SpawnLocations.Add(entityToSpawn.Position);
                break;
            case MapDTO.EntityType.ToolAxe:
                TDObject toolAxeObject = PrefabFactory.CreatePrefab(PrefabType.ToolAxe, ItemTransform);
                toolAxeObject.GetComponent<ToolAxe>().Position = entityToSpawn.Position;
                break;
            case MapDTO.EntityType.ToolPickaxe:
                TDObject toolPickaxeObject = PrefabFactory.CreatePrefab(PrefabType.ToolPickaxe, ItemTransform);
                toolPickaxeObject.GetComponent<ToolPickaxe>().Position = entityToSpawn.Position;
                break;
            case MapDTO.EntityType.ToolHammer:
                TDObject toolHammerObject = PrefabFactory.CreatePrefab(PrefabType.ToolHammer, ItemTransform);
                toolHammerObject.GetComponent<ToolHammer>().Position = entityToSpawn.Position;
                break;
            case MapDTO.EntityType.WeaponSword:
                TDObject weaponSwordObject = PrefabFactory.CreatePrefab(PrefabType.WeaponSword, ItemTransform);
                weaponSwordObject.GetComponent<WeaponSword>().Position = entityToSpawn.Position;
                break;
            case MapDTO.EntityType.None:
            default:
                throw new NotSupportedException();
        }
    }
}
