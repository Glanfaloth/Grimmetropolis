using Microsoft.Xna.Framework;

public class GameScene : TDScene
{
    public GameScene() { }

    public override void Initialize()
    {
        // TEST SCENE 1
        /*PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(1f, 1f, 1f), Quaternion.CreateFromYawPitchRoll(0f, .25f * MathHelper.Pi, 1.3f * MathHelper.Pi));

        TDObject center = PrefabFactory.CreatePrefab(PrefabType.Empty);
        center.Components.Add(new TestComponent(center));

        TDObject mainBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(0f, 0f, -.5f), Quaternion.Identity, center.Transform);
        mainBlock.Components.Add(new TDCuboidCollider(mainBlock, Vector3.One, new Vector3(0f, 0f, .5f)));

        TDObject sideBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(0f, 1f, 0f), Quaternion.Identity, mainBlock.Transform);
        sideBlock.Components.Add(new TDCuboidCollider(sideBlock, Vector3.One, new Vector3(0f, 0f, .5f)));*/

        // TEST SCENE 2
        PrefabFactory.CreatePrefab(PrefabType.Camera, new Vector3(2f, 0f, 6f), Quaternion.CreateFromYawPitchRoll(-.4f * MathHelper.Pi, 0f, MathHelper.Pi));

        TDObject mainBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(-1.5f, -1.5f, 0f), Quaternion.Identity);
        mainBlock.Components.Add(new TDCuboidCollider(mainBlock, false, Vector3.One, new Vector3(0f, 0f, .5f)));

        TDObject[] calmCylinders = new TDObject[10];
        for (int i = 0; i < calmCylinders.Length; i++)
        {
            calmCylinders[i] = PrefabFactory.CreatePrefab(PrefabType.Empty, new Vector3(1.5f, i - 5f, 0f), Quaternion.Identity);
            calmCylinders[i].Components.Add(new TDMesh(calmCylinders[i], TDContentManager.LoadModel("DefaultCylinder"), TDContentManager.LoadTexture("DefaultTexture")));
            calmCylinders[i].Components.Add(new TDCylinderCollider(calmCylinders[i], false, .5f, 1f, new Vector3(0f, 0f, .5f)));
        }

        TDObject cylinder = PrefabFactory.CreatePrefab(PrefabType.Empty, new Vector3(0f, 0f, .5f), Quaternion.Identity);
        cylinder.Components.Add(new TDMesh(cylinder, TDContentManager.LoadModel("DefaultCylinder"), TDContentManager.LoadTexture("DefaultTexture")));
        TDCylinderCollider cylinderCollider = new TDCylinderCollider(cylinder, false, .5f, 1f, new Vector3(0f, 0f, .5f));
        cylinder.Components.Add(cylinderCollider);
        cylinder.Components.Add(new MoveComponent(cylinder));
    }
}
