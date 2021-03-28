using Microsoft.Xna.Framework;

public class TDCuboidCollider : TDCollider
{
    public Vector3 Size = Vector3.One;
    public Vector3 Offset = Vector3.Zero;

    public Vector3 CuboidCornerLow { get; private set; }
    public Vector3 CuboidCornerHigh { get; private set; }

    public override void UpdateCollision()
    {
        base.UpdateCollision();

        CuboidCornerLow = TDObject.Transform.Position - .5f * Vector3.Multiply(TDObject.Transform.Scale, Size) + Offset;
        CuboidCornerHigh = CuboidCornerLow + Vector3.Multiply(TDObject.Transform.Scale, Size);
    }

    public override void Collide(TDCollider collider)
    {
        if (collider is TDCylinderCollider cylinderCollider)
        {
            CollideCylinderCuboid(cylinderCollider, this);
        }
    }
}