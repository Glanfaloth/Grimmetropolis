using Microsoft.Xna.Framework;

using System;

public abstract class TDCollider : TDComponent
{

    public bool IsColliding;
    public bool IsTrigger;

    public TDCollider(TDObject tdObject, bool isTrigger) : base(tdObject)
    {
        IsColliding = false;
        IsTrigger = isTrigger;

        TDSceneManager.ActiveScene.ColliderObjects.Add(this);
    }

    public abstract void UpdateCollision(TDCollider collider);

    protected bool CylinderCylinderCollision(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2)
    {
        cylinder1.CalculateCylinder();
        cylinder2.CalculateCylinder();

        if (cylinder1.CenterZLow < cylinder2.CenterZHigh && cylinder1.CenterZHigh > cylinder2.CenterZLow)
        {
            float intersection = cylinder1.Radius + cylinder2.Radius - Vector2.Distance(cylinder1.CenterXY, cylinder2.CenterXY);
            if (intersection < 0f) return false;

            if (!cylinder1.IsTrigger && !cylinder2.IsTrigger)
            {
                Vector3 direction = new Vector3(.5f * intersection * Vector2.Normalize(cylinder1.CenterXY - cylinder2.CenterXY), 0f);

                cylinder1.TDObject.Transform.LocalPosition += direction;
                cylinder2.TDObject.Transform.LocalPosition -= direction;
            }
            else return true;
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

            float intersection = cylinder.Radius - Vector2.Distance(closest, cylinder.CenterXY);
            if (intersection < 0f) return false;

            if (!cylinder.IsTrigger && !cuboid.IsTrigger)
            {
                Vector3 direction = new Vector3(intersection * Vector2.Normalize(cylinder.CenterXY - closest), 0f);

                cylinder.TDObject.Transform.LocalPosition += direction;
            }
            else return true;
        }

        return false;
    }
}
