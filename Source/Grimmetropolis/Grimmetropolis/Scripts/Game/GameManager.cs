﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class GameManager : TDComponent
{
    public static GameManager Instance;

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

    public EnemyController EnemyController;
    public List<Player> Players = new List<Player>();
    public List<Enemy> Enemies = new List<Enemy>();
    public List<Structure> Structures = new List<Structure>();
    public List<Item> Items = new List<Item>();

    public TDTransform PlayerTransform;
    public TDTransform EnemyTransform;
    public TDTransform StructureTransform;
    public TDTransform ItemTransform;

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
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/test.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testAvoidOutpost.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testAttackOutpost.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testEmpty64.map");
        // List<MapDTO.EntityToSpawn> entitiesToSpawn = Map.LoadFromFile("Content/Maps/testEmpty128.map");

        // ResourcePool
        ResourcePool = new ResourcePile();

        // EnemyBrain
        TDObject enemyAI = PrefabFactory.CreatePrefab(PrefabType.Empty);
        EnemyController = enemyAI.AddComponent<EnemyController>();

        // Spawner
        // TODO: replace test spawner with real spawner
        TDObject testSpawner = PrefabFactory.CreatePrefab(PrefabType.Empty);
        var spawner = testSpawner.AddComponent<TestSpawner>();

        // Players
        TDObject playerList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        PlayerTransform = playerList.Transform;

        TDObject playerObject0 = PrefabFactory.CreatePrefab(PrefabType.Player, playerList.Transform);
        playerObject0.GetComponent<Player>().Input = TDInputManager.Inputs[0];

        TDObject playerObject1 = PrefabFactory.CreatePrefab(PrefabType.Player, playerList.Transform);
        playerObject1.GetComponent<Player>().Input = TDInputManager.Inputs[1];

        // Enemies
        TDObject enemyList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        EnemyTransform = enemyList.Transform;

        // Structures
        TDObject structureList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        StructureTransform = structureList.Transform;

        foreach (var entityToSpawn in entitiesToSpawn)
        {
            SpawnEntity(spawner, enemyList, entityToSpawn);
        }

        if (spawner.SpawnLocations.Count == 0)
        {
            spawner.SpawnLocations.Add(Point.Zero);
        }

        // Items
        TDObject itemList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        ItemTransform = itemList.Transform;

        TDObject pickaxeObject = PrefabFactory.CreatePrefab(PrefabType.ToolPickaxe, ItemTransform);
        ToolPickaxe pickaxe = pickaxeObject.GetComponent<ToolPickaxe>();
        pickaxe.Character = playerObject1.GetComponent<Player>();

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
    }

    private void SpawnEntity(TestSpawner spawner, TDObject enemyList, MapDTO.EntityToSpawn entityToSpawn)
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
                    Map.EnemyTarget = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.EnemyWitch:
                PrefabFactory.CreateEnemyPrefab<EnemyWitch>(Config.ENEMY_WITCH_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                    Quaternion.Identity, enemyList.Transform);
                break;
            case MapDTO.EntityType.EnemyKnight:
                PrefabFactory.CreateEnemyPrefab<EnemyKnight>(Config.ENEMY_KNIGHTS_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                    Quaternion.Identity, enemyList.Transform);
                break;
            case MapDTO.EntityType.EnemyCatapult:
                PrefabFactory.CreateEnemyPrefab<EnemyCatapult>(Config.ENEMY_CATAPULT_STATS, Map.Corner + entityToSpawn.Position.ToVector3() + Map.Offcenter,
                    Quaternion.Identity, enemyList.Transform);
                break;
            case MapDTO.EntityType.Outpost:
                {
                    TDObject newEntity = PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, StructureTransform);
                    newEntity.GetComponent<Outpost>().Position = entityToSpawn.Position;
                }
                break;
            case MapDTO.EntityType.EnemySpawnPoint:
                spawner.SpawnLocations.Add(entityToSpawn.Position);
                break;
            case MapDTO.EntityType.None:
            default:
                throw new NotSupportedException();
        }
    }
}
