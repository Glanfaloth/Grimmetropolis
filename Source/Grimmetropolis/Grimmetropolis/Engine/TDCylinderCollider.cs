using Microsoft.Xna.Framework;

using System.Diagnostics;

public class TDCylinderCollider : TDCollider
{
    public float Radius;
    public float Height;
    public Vector3 Offset;

    public Vector2 CenterXY { get; private set; }
    public float CenterZLow { get; private set; }
    public float CenterZHigh { get; private set; }

    public TDCylinderCollider(TDObject tdObject, float radius, float height, Vector3 offset) : base(tdObject)
    {
        Radius = radius;
        Height = height;
        Offset = offset;
    }

    public override void UpdateCollision(TDCollider collider)
    {
        if (collider is TDCuboidCollider cuboidCollider)
        {
            IsColliding = CylinderCuboidCollision(this, cuboidCollider);
            if (IsColliding) ResolveCylinderCuboidCollision(this, cuboidCollider);
        }

        if (collider is TDCylinderCollider cylinderCollider)
        {
            IsColliding = CylinderCylinderCollision(this, cylinderCollider);
            if (IsColliding) ResolveCylinderCylinderCollision(this, cylinderCollider);
        }

        collider.IsColliding = IsColliding;
    }

    public void CalculateCylinder()
    {
        Vector3 Center = TDObject.Transform.Position + Offset;
        CenterXY = new Vector2(Center.X, Center.Y);
        CenterZLow = Center.Z - .5f * TDObject.Transform.Scale.Z * Height;
        CenterZHigh = CenterZLow + TDObject.Transform.Scale.Z * Height;
    }
}