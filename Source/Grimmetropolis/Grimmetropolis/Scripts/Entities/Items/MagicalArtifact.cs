using Microsoft.Xna.Framework;

public class MagicalArtifact : Item
{

    public Castle Castle = null;

    public override void Initialize()
    {
        base.Initialize();

        CarryPosition = new Vector3(0f, 0f, .9f);
        CarryRotation = Quaternion.Identity;
        CarryScale = .75f * Vector3.One;
    }
    protected override void SetMapTransform()
    {
        if (Castle != null) Position = Castle.Position + new Point(1, 1);

        base.SetMapTransform();

        if (Castle != null)
        {
            Vector3 position = TDObject.Transform.Position;
            position.Z = 3f;
            TDObject.Transform.Position = position;
        }
    }
}
