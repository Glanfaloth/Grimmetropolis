using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        base.Initialize();

        // TEST SCENE 1
        /*PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(1f, 1f, 1f), Quaternion.CreateFromYawPitchRoll(0f, .25f * MathHelper.Pi, 1.3f * MathHelper.Pi));

        TDObject center = PrefabFactory.CreatePrefab(PrefabType.Empty);
        center.Components.Add(new TestComponent(center));

        TDObject mainBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(0f, 0f, -.5f), Quaternion.Identity, center.Transform);
        mainBlock.Components.Add(new TDCuboidCollider(mainBlock, false, Vector3.One, new Vector3(0f, 0f, .5f)));

        TDObject sideBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(0f, 1f, 0f), Quaternion.Identity, mainBlock.Transform);
        sideBlock.Components.Add(new TDCuboidCollider(sideBlock, false, Vector3.One, new Vector3(0f, 0f, .5f)));*/

        // TEST SCENE 2
        /*PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(2f, 0f, 6f), Quaternion.CreateFromYawPitchRoll(-.4f * MathHelper.Pi, 0f, MathHelper.Pi));

        TDObject mainBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(-1.5f, -1.5f, 0f), Quaternion.Identity);
        mainBlock.Components.Add(new TDCuboidCollider(mainBlock, false, Vector3.One, new Vector3(0f, 0f, .5f)));

        TDObject[] calmCylinders = new TDObject[10];
        for (int i = 0; i < calmCylinders.Length; i++)
        {
            calmCylinders[i] = PrefabFactory.CreatePrefab(PrefabType.Empty, new Vector3(1.5f, i - 5f, 0f), Quaternion.Identity);
            calmCylinders[i].Components.Add(new TDMesh(calmCylinders[i], "DefaultCylinder", "DefaultTexture"));
            calmCylinders[i].Components.Add(new TDCylinderCollider(calmCylinders[i], false, .5f, 1f, new Vector3(0f, 0f, .5f)));
        }

        TDObject cylinder = PrefabFactory.CreatePrefab(PrefabType.Empty);
        cylinder.Components.Add(new TDMesh(cylinder, "DefaultCylinder", "DefaultTexture"));
        TDCylinderCollider cylinderCollider = new TDCylinderCollider(cylinder, false, .5f, 1f, new Vector3(0f, 0f, .5f));
        cylinder.Components.Add(cylinderCollider);
        cylinder.Components.Add(new MoveComponent(cylinder));*/

        // TEST SCENE 3
        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(2f, 0f, 6f), Quaternion.CreateFromYawPitchRoll(-.4f * MathHelper.Pi, 0f, MathHelper.Pi));
        PrefabFactory.CreatePrefab(PrefabType.Light, new Vector3(4f, 4f, 6f), Quaternion.CreateFromYawPitchRoll(-.2f * MathHelper.Pi, .2f * MathHelper.Pi, -.8f * MathHelper.Pi));

        TDObject groundBlock = PrefabFactory.CreatePrefab(PrefabType.Default, Vector3.Zero, Quaternion.CreateFromYawPitchRoll(MathHelper.Pi, 0f, 0f));
        groundBlock.Transform.LocalScale *= 8f;

        TDObject movingCylinder = PrefabFactory.CreatePrefab(PrefabType.Empty);
        movingCylinder.Components.Add(new TDMesh(movingCylinder, "DefaultCylinder", "DefaultTexture"));
        movingCylinder.Components.Add(new TDCylinderCollider(movingCylinder, false, .5f, 1f, .5f * Vector3.Backward));
        movingCylinder.Components.Add(new MoveComponent(movingCylinder));

        TDObject cylinder = PrefabFactory.CreatePrefab(PrefabType.Empty, 1.5f * Vector3.Down, Quaternion.Identity);
        cylinder.Components.Add(new TDMesh(cylinder, "DefaultCylinder", "DefaultTexture"));
        cylinder.Components.Add(new TDCylinderCollider(cylinder, false, .5f, 1f, .5f * Vector3.Backward));

        TDObject block = PrefabFactory.CreatePrefab(PrefabType.Default, 1.5f * Vector3.Up, Quaternion.Identity);
        block.Components.Add(new TDCuboidCollider(block, false, Vector3.One, .5f * Vector3.Backward));
    }
}
