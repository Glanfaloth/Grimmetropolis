using Microsoft.Xna.Framework;

public class GameManager : TDComponent
{
    public static GameManager Instance;

    public Map Map;
    public EnemyController EnemyController;

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
        EnemyController.Map = Map;
        
        // Players
        TDObject player = PrefabFactory.CreatePrefab(PrefabType.Player);
        player.GetComponent<Player>().Input = TDInputManager.Inputs[0];

        // Enemies
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-3f, -1f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, -2f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-6f, 4f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, 6f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-2f, 2f, 0f), Quaternion.Identity);
    }

}
