﻿using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        // TEST SCENE 3
        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(2f, 0f, 6f), Quaternion.CreateFromYawPitchRoll(-.4f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(4f, 4f, 6f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));

        PrefabFactory.CreatePrefab(PrefabType.Map);
        
        PrefabFactory.CreatePrefab(PrefabType.Player);
        PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-3f, .5f, 0f), Quaternion.Identity);

        TDObject block = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(-1f, .5f, 0f), Quaternion.Identity);
        block.Components.Add(new TDCuboidCollider(block, false, Vector3.One, .5f * Vector3.Backward));
    }
}
