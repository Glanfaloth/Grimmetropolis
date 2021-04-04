using Microsoft.Xna.Framework;

using System;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(6f, 0f, 18f), Quaternion.CreateFromYawPitchRoll(-.375f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(12f, 12f, 18f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));

        PrefabFactory.CreatePrefab(PrefabType.GameManager);

        TDObject testSpawner = PrefabFactory.CreatePrefab(PrefabType.Empty);
        testSpawner.AddComponent<TestSpawner>();

        TDObject testImage = PrefabFactory.CreatePrefab(PrefabType.Empty2D, new Vector3(10f, 10f, 0f), Quaternion.CreateFromAxisAngle(Vector3.Backward, .1f), new Vector3(.5f, .5f, 1f), null);
        TDSprite spriteComponent = testImage.AddComponent<TDSprite>();
        spriteComponent.Texture = TDContentManager.LoadTexture("DefaultTexture");
        spriteComponent.Color = Color.White;
        testImage.RectTransform.Scale = .4f * Vector2.One;

        TDObject testImage0 = PrefabFactory.CreatePrefab(PrefabType.Empty2D, testImage.Transform);
        TDSprite spriteComponent0 = testImage0.AddComponent<TDSprite>();
        spriteComponent0.Texture = TDContentManager.LoadTexture("DefaultTexture");
        spriteComponent0.Color = Color.White;
        testImage0.RectTransform.LocalScale = .5f * Vector2.One;
        testImage0.RectTransform.Rotation = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Update cuboid collision
        foreach (TDCylinderCollider cylinder in CylinderColliderObjects)
        {
            cylinder.UpdateColliderGeometry();
            int minX = Math.Max(cylinder.MapPosition.X - 1, 0); int maxX = Math.Min(cylinder.MapPosition.X + 1, GameManager.Instance.Map.Width - 1);
            int minY = Math.Max(cylinder.MapPosition.Y - 1, 0); int maxY = Math.Min(cylinder.MapPosition.Y + 1, GameManager.Instance.Map.Height - 1);
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    cylinder.UpdateCollision(CuboidColliderObjects[x, y]);
                }
            }
        }
    }
}
