using Microsoft.Xna.Framework;

public class GameScene : TDScene
{

    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(8f, 0f, 24f), Quaternion.CreateFromYawPitchRoll(-.375f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(12f, 12f, 18f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));

        PrefabFactory.CreatePrefab(PrefabType.GameManager);
        
        PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, new Vector3(0f, 4.5f, 0f), Quaternion.Identity);
    }
}
