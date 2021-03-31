using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class GameManager : TDComponent
{
    public static GameManager Instance;

    public Map Map;
    public ResourcePile ResourcePool;

    public EnemyController EnemyController;
    public List<Player> Players = new List<Player>();
    public List<Enemy> Enemies = new List<Enemy>();
    public List<Structure> Structures = new List<Structure>();

    public TDTransform PlayerTransform;
    public TDTransform EnemyTransform;
    public TDTransform StructureTransform;

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        // Map
        TDObject mapObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        Map = mapObject.AddComponent<Map>();

        // ResourcePool
        ResourcePool = new ResourcePile();

        // EnemyBrain
        TDObject enemyAI = PrefabFactory.CreatePrefab(PrefabType.Empty);
        EnemyController = enemyAI.AddComponent<EnemyController>();

        // Players
        TDObject playerList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        PlayerTransform = playerList.Transform;

        TDObject playerObject = PrefabFactory.CreatePrefab(PrefabType.Player, playerList.Transform);
        playerObject.GetComponent<Player>().Input = TDInputManager.DefaultInput;

        // Enemies
        TDObject enemyList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        EnemyTransform = enemyList.Transform;

        Vector3 offset = new Vector3( .5f, .5f, 0);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-2f, -3f, 0f) + offset, Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(4f, -4f, 0f) + offset, Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-6f, 4f, 0f) + offset, Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, 6f, 0f) + offset, Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-2f, 1f, 0f) + offset, Quaternion.Identity, enemyList.Transform);

        // Structures
        TDObject structureList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        StructureTransform = structureList.Transform;

        TDObject castleObject = PrefabFactory.CreatePrefab(PrefabType.Castle, StructureTransform);
        Castle castle = castleObject.GetComponent<Castle>();
        castle.Position = new Point(3, 8);

        TDObject resourceWoodObject0 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        ResourceDeposit resourceWood0 = resourceWoodObject0.GetComponent<ResourceDeposit>();
        resourceWood0.Position = new Point(6, 4);
        TDObject resourceWoodObject1 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        ResourceDeposit resourceWood1 = resourceWoodObject1.GetComponent<ResourceDeposit>();
        resourceWood1.Position = new Point(7, 4);
        TDObject resourceWoodObject2 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        ResourceDeposit resourceWood2 = resourceWoodObject2.GetComponent<ResourceDeposit>();
        resourceWood2.Position = new Point(6, 5);
        TDObject resourceWoodObject3 = PrefabFactory.CreatePrefab(PrefabType.Wood, StructureTransform);
        ResourceDeposit resourceWood3 = resourceWoodObject3.GetComponent<ResourceDeposit>();
        resourceWood3.Position = new Point(7, 5);
        TDObject resourceStoneObject = PrefabFactory.CreatePrefab(PrefabType.Stone, StructureTransform);
        ResourceDeposit resourceStone = resourceStoneObject.GetComponent<ResourceDeposit>();
        resourceStone.Position = new Point(7, 10);
    }

}
