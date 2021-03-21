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

    // TODO: Collision detection and resolve need a lot of optimization!

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

    protected bool CylinderCuboidCollision(TDCylinderCollider cylinder, TDCuboidCollider cuboid)
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

    protected void ResolveCylinderCylinderCollision(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2)
    {
        float intersection = cylinder1.Radius + cylinder2.Radius - Vector2.Distance(cylinder1.CenterXY, cylinder2.CenterXY);
        Vector3 direction = new Vector3(.5f * intersection * Vector2.Normalize(cylinder1.CenterXY - cylinder2.CenterXY), 0f);

        cylinder1.TDObject.Transform.LocalPosition += direction;
        cylinder2.TDObject.Transform.LocalPosition -= direction;

        cylinder1.IsColliding = false;
        cylinder2.IsColliding = false;
    }

    protected void ResolveCylinderCuboidCollision(TDCylinderCollider cylinder, TDCuboidCollider cuboid)
    {
        Vector2 closest = new Vector2(MathHelper.Clamp(cylinder.CenterXY.X, cuboid.CuboidCornerLow.X, cuboid.CuboidCornerHigh.X),
                MathHelper.Clamp(cylinder.CenterXY.Y, cuboid.CuboidCornerLow.Y, cuboid.CuboidCornerHigh.Y));

        float intersection = cylinder.Radius - Vector2.Distance(closest, cylinder.CenterXY);
        Vector3 direction = new Vector3(intersection * Vector2.Normalize(cylinder.CenterXY - closest), 0f);

        cylinder.TDObject.Transform.LocalPosition += direction;

        cylinder.IsColliding = false;
        cuboid.IsColliding = false;
    }
}
