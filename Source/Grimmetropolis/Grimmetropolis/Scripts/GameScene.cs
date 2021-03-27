using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        TDObject map = PrefabFactory.CreatePrefab(PrefabType.Empty);
        var mapComponent = new Map(map);
        map.Components.Add(mapComponent);
        PrefabFactory.InitializeEnemyController(mapComponent);

        // TEST SCENE 3
        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(2f, 0f, 6f), Quaternion.CreateFromYawPitchRoll(-.4f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(4f, 4f, 6f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));
        
        PrefabFactory.CreatePrefab(PrefabType.Player);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-3f, -1f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, -2f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-6f, 4f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(3f, 6f, 0f), Quaternion.Identity);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-2f, 2f, 0f), Quaternion.Identity);


        TDObject block = PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, new Vector3(0f, 4.5f, 0f), Quaternion.Identity);
    }
}
