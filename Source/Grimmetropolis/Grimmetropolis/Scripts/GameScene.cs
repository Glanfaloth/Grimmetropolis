using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(4f, 0f, 12f), Quaternion.CreateFromYawPitchRoll(-.375f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(8f, 8f, 12f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));

        TDObject map = PrefabFactory.CreatePrefab(PrefabType.Empty);
        map.AddComponent<Map>();
        
        TDObject player = PrefabFactory.CreatePrefab(PrefabType.Player);
        player.GetComponent<Player>().Input = TDInputManager.Inputs[0];

        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(.5f, -3f, 0f), Quaternion.Identity);
        
        PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, new Vector3(0f, 4.5f, 0f), Quaternion.Identity);
    }
}
