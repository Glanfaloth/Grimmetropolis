using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(1f, 1f, 1f), Quaternion.CreateFromYawPitchRoll(0f, 0.25f * MathHelper.Pi, 1.3f * MathHelper.Pi));
        TDObject defaultMain = PrefabFactory.CreatePrefab(PrefabType.DefaultKeyInput);
        PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(0f, 0f, 1f), Quaternion.Identity, defaultMain.Transform);
    }
}
