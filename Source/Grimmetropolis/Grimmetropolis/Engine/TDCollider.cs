using Microsoft.Xna.Framework;

using System;

public abstract class TDCollider : TDComponent
{

    public bool IsColliding;

    public TDCollider(TDObject tdObject) : base(tdObject)
    {
        IsColliding = false;

        TDSceneManager.ActiveScene.ColliderObjects.Add(this);
    }

    public abstract void UpdateCollision(TDCollider collider);

    protected bool CuboidCuboidCollision(TDCuboidCollider cuboid1, TDCuboidCollider cuboid2)
    {
        cuboid1.CalculateCuboid();
        cuboid2.CalculateCuboid();

        return cuboid1.CuboidCornerLow.X < cuboid2.CuboidCornerHigh.X
            && cuboid1.CuboidCornerHigh.X > cuboid2.CuboidCornerLow.X
            && cuboid1.CuboidCornerLow.Y < cuboid2.CuboidCornerHigh.Y
            && cuboid1.CuboidCornerHigh.Y > cuboid2.CuboidCornerLow.Y
            && cuboid1.CuboidCornerLow.Z < cuboid2.CuboidCornerHigh.Z
            && cuboid1.CuboidCornerHigh.Z > cuboid2.CuboidCornerLow.Z;
    }
    protected bool CylinderCylinderCollision(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2)
    {
        cylinder1.CalculateCylinder();
        cylinder2.CalculateCylinder();

        if (cylinder1.CenterZLow < cylinder2.CenterZHigh && cylinder1.CenterZHigh > cylinder2.CenterZLow)
        {
            return Vector2.DistanceSquared(cylinder1.CenterXY, cylinder2.CenterXY) < MathF.Pow(cylinder1.Radius + cylinder2.Radius, 2f);
        }

        return false;
    }
    protected bool CuboidCylinderCollision(TDCuboidCollider cuboid, TDCylinderCollider cylinder)
    {
        cuboid.CalculateCuboid();
        cylinder.CalculateCylinder();

        if (cuboid.CuboidCornerLow.Z < cylinder.CenterZHigh && cuboid.CuboidCornerHigh.Z > cylinder.CenterZLow)
        {
            Vector2 closest = new Vector2(MathHelper.Clamp(cylinder.CenterXY.X, cuboid.CuboidCornerLow.X, cuboid.CuboidCornerHigh.X),
                MathHelper.Clamp(cylinder.CenterXY.Y, cuboid.CuboidCornerLow.Y, cuboid.CuboidCornerHigh.Y));

            return Vector2.DistanceSquared(cylinder.CenterXY, closest) < MathF.Pow(cylinder.Radius, 2f);
        }

        return false;
    }
}
