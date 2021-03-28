using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class GameManager : TDComponent
{
    public static GameManager Instance;

    public Map Map;
    public EnemyController EnemyController;
    public List<Player> Players = new List<Player>();
    public List<Enemy> Enemies = new List<Enemy>();
    public List<Building> Buildings = new List<Building>();
    public List<Resource> Resources = new List<Resource>();

    public TDTransform PlayerTransform;
    public TDTransform EnemyTransform;
    public TDTransform BuildingTransform;
    public TDTransform ResourceTransform;

    public float WoodResource = 0f;
    public float StoneResource = 0f;

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        // Map
        TDObject mapObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        Map = mapObject.AddComponent<Map>();

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

        // Buildings
        TDObject buildingList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        BuildingTransform = buildingList.Transform;

        TDObject castleObject = PrefabFactory.CreatePrefab(PrefabType.Castle, BuildingTransform);
        Castle castle = castleObject.GetComponent<Castle>();
        castle.Position = new Point(3, 8);

        // Resources
        TDObject resourceList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        ResourceTransform = resourceList.Transform;

        TDObject resourceWoodObject0 = PrefabFactory.CreatePrefab(PrefabType.Wood, ResourceTransform);
        Resource resourceWood0 = resourceWoodObject0.GetComponent<Resource>();
        resourceWood0.Position = new Point(6, 4);
        TDObject resourceWoodObject1 = PrefabFactory.CreatePrefab(PrefabType.Wood, ResourceTransform);
        Resource resourceWood1 = resourceWoodObject1.GetComponent<Resource>();
        resourceWood1.Position = new Point(7, 4);
        TDObject resourceWoodObject2 = PrefabFactory.CreatePrefab(PrefabType.Wood, ResourceTransform);
        Resource resourceWood2 = resourceWoodObject2.GetComponent<Resource>();
        resourceWood2.Position = new Point(6, 5);
        TDObject resourceWoodObject3 = PrefabFactory.CreatePrefab(PrefabType.Wood, ResourceTransform);
        Resource resourceWood3 = resourceWoodObject3.GetComponent<Resource>();
        resourceWood3.Position = new Point(7, 5);

        TDObject resourceStoneObject = PrefabFactory.CreatePrefab(PrefabType.Stone, ResourceTransform);
        Resource resourceStone = resourceStoneObject.GetComponent<Resource>();
        resourceStone.Position = new Point(7, 10);
    }

}
