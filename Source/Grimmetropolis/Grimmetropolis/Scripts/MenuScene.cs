using Microsoft.Xna.Framework;


public class MenuScene : TDScene
{
    public MenuScene() { }

    public override void Initialize()
    {
        base.Initialize();

        Background = TDContentManager.LoadTexture("BackgroundBlurred");

        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(10f, 0f, 0f), Quaternion.CreateFromYawPitchRoll(0f, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(24f, 24f, 36f), Quaternion.CreateFromYawPitchRoll(-.31f * MathHelper.Pi, .1f * MathHelper.Pi, -.85f * MathHelper.Pi));

        PrefabFactory.CreatePrefab(PrefabType.MenuUIManager);
    }
}
