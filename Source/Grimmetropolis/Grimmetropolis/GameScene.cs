using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        // TDObjects.Add(new Prefab("Camera", new Vector3(1f, 1f, 1f), Quaternion.CreateFromYawPitchRoll(0f, 4.1f, 2.2f)));
        TDObjects.Add(new Prefab("Camera", new Vector3(1f, 1f, 1f), Quaternion.CreateFromYawPitchRoll(0f, 0.25f * MathHelper.Pi, 1.3f * MathHelper.Pi)));
        TDObjects.Add(new Prefab("Default"));
    }
}
