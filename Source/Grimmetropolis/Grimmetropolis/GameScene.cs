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

        TDObject mainBlock = PrefabFactory.CreatePrefab(PrefabType.Default, new Vector3(1.5f, 1.5f, 0f), Quaternion.Identity);
        mainBlock.Components.Add(new TDCuboidCollider(mainBlock, Vector3.One, new Vector3(0f, 0f, .5f)));

        TDObject cylinder = PrefabFactory.CreatePrefab(PrefabType.Empty);
        cylinder.Components.Add(new TDMesh(cylinder, TDContentManager.LoadModel("DefaultCylinder"), TDContentManager.LoadTexture("DefaultTexture")));
        TDCylinderCollider cylinderCollider = new TDCylinderCollider(cylinder, .5f, 1f, new Vector3(0f, 0f, .5f));
        cylinder.Components.Add(cylinderCollider);
        cylinder.Components.Add(new MoveComponent(cylinder, cylinderCollider));
    }
}
