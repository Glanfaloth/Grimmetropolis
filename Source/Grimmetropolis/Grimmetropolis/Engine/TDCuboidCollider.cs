using Microsoft.Xna.Framework;

using System.Diagnostics;

public class TDCuboidCollider : TDCollider
{
    public Vector3 Size;
    public Vector3 Offset;

    public Vector3 CuboidCornerLow { get; private set; }
    public Vector3 CuboidCornerHigh { get; private set; }

    public TDCuboidCollider(TDObject tdObject, Vector3 size, Vector3 offset) : base(tdObject)
    {
        Size = size;
        Offset = offset;
    }

    public override void UpdateCollision(TDCollider collider)
    {
        if (collider is TDCylinderCollider cylinderCollider)
        {
            IsColliding = CylinderCuboidCollision(cylinderCollider, this);
            if (IsColliding) ResolveCylinderCuboidCollision(cylinderCollider, this);
        }

        collider.IsColliding = IsColliding;
    }

    public void CalculateCuboid()
    {
        CuboidCornerLow = TDObject.Transform.Position - .5f * Vector3.Multiply(TDObject.Transform.Scale, Size) + Offset;
        CuboidCornerHigh = CuboidCornerLow + Vector3.Multiply(TDObject.Transform.Scale, Size);
    }
}