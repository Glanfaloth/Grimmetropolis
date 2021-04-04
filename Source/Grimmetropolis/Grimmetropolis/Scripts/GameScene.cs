﻿using Microsoft.Xna.Framework;

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

        TDObject testImage = PrefabFactory.CreatePrefab(PrefabType.EmptyUI, new Vector3(10f, 10f, 0f), Quaternion.CreateFromAxisAngle(Vector3.Backward, .1f), new Vector3(.5f, .5f, 1f), null);
        TDSprite spriteComponent = testImage.AddComponent<TDSprite>();
        spriteComponent.Texture = TDContentManager.LoadTexture("DefaultTexture");
        spriteComponent.Color = Color.White;
        spriteComponent.Depth = .1f;
        testImage.RectTransform.Scale = .4f * Vector2.One;

        TDObject testImage0 = PrefabFactory.CreatePrefab(PrefabType.EmptyUI, testImage.Transform);
        TDSprite spriteComponent0 = testImage0.AddComponent<TDSprite>();
        spriteComponent0.Texture = TDContentManager.LoadTexture("DefaultTexture");
        spriteComponent0.Color = Color.White;
        spriteComponent0.Depth = 0;
        testImage0.RectTransform.LocalScale = .5f * Vector2.One;
        testImage0.RectTransform.Rotation = 0f;

        TDObject player1 = PrefabFactory.CreatePrefab(PrefabType.Player);
        Player playerComponent = player1.GetComponent<Player>();
        playerComponent.Input = TDInputManager.DefaultInput;

        TDObject testImage1 = PrefabFactory.CreatePrefab(PrefabType.EmptyUI3D, player1.Transform);
        TDText textComponent1 = testImage1.AddComponent<TDText>();
        textComponent1.SpriteFont = TDContentManager.LoadSpriteFont("Montserrat");
        textComponent1.Text = "Player oder so...";
        textComponent1.Color = Color.White;
        textComponent1.Depth = 1f;
        testImage1.RectTransform.Offset = 4f * Vector3.Backward;
        testImage1.RectTransform.Origin = new Vector2(.5f * textComponent1.Width, textComponent1.Height);
        // testImage1.RectTransform.LocalScale = 1f * Vector2.One;
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
