using Microsoft.Xna.Framework;

public class Prefab : TDObject
{

    public Prefab(string name, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDObject parent)
        : base(localPosition, localRotation, localScale, parent)
    {
        switch (name)
        {
            case "Camera":
                Components.Add(new TDCameraComponent(this, .5f * MathHelper.Pi, .1f, 100f));
                break;
            case "Default":
                Components.Add(new TDMeshComponent(this, TDContentManager.LoadModel("DefaultModel"), TDContentManager.LoadTexture("DefaultTexture")));
                Components.Add(new TestComponent(this));
                break;
        }
    }

    public Prefab(string name) : this(name, Vector3.Zero, Quaternion.Identity, Vector3.One, null) { }

    public Prefab(string name, Vector3 localPosition, Quaternion localRotation)
        : this(name, localPosition, localRotation, Vector3.One, null) { }

    public Prefab(string name, TDObject parent) : this(name, Vector3.Zero, Quaternion.Identity, Vector3.One, parent) { }

    public Prefab(string name, Vector3 localPosition, Quaternion localRotation, TDObject parent)
        : this(name, localPosition, localRotation, Vector3.One, parent) { }

}
