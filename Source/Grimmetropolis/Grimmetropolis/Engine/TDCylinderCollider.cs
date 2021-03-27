﻿using Microsoft.Xna.Framework;

using System.Diagnostics;

public class TDCylinderCollider : TDCollider
{
    public float Radius = .5f;
    public float Height = 1f;
    public Vector3 Offset = Vector3.Zero;

    public Vector2 CenterXY { get; private set; }
    public float CenterZLow { get; private set; }
    public float CenterZHigh { get; private set; }

    public override void UpdateCollision()
    {
        base.UpdateCollision();

        Vector3 Center = TDObject.Transform.Position + Offset;
        CenterXY = new Vector2(Center.X, Center.Y);
        CenterZLow = Center.Z - .5f * TDObject.Transform.Scale.Z * Height;
        CenterZHigh = CenterZLow + TDObject.Transform.Scale.Z * Height;
    }

    public override void Collide(TDCollider collider)
    {
        if (collider is TDCuboidCollider cuboidCollider)
        {
            CollideCylinderCuboid(this, cuboidCollider);
        }
        else if (collider is TDCylinderCollider cylinderCollider)
        {
            CollideCylinderCylinder(this, cylinderCollider);
        }
    }
}