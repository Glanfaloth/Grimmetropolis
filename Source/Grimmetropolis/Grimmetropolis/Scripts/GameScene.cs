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

        TDObject building = PrefabFactory.CreatePrefab(PrefabType.Empty);
        building.AddComponent<TestSpawner>();
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
