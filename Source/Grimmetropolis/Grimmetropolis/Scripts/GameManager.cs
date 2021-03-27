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

    public TDTransform PlayerTransform;
    public TDTransform EnemyTransform;
    public TDTransform BuildingTransform;

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

        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-8f, -4f, 0f), Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(4f, -4f, 0f), Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-6f, 4f, 0f), Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, 6f, 0f), Quaternion.Identity, enemyList.Transform);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-2f, 2f, 0f), Quaternion.Identity, enemyList.Transform);

        // Buildings
        TDObject buildingList = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        BuildingTransform = buildingList.Transform;
    }

}
