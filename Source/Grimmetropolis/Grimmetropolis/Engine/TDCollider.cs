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

    public virtual void UpdateCollision()
    {
        IsColliding = false;
    }

    public abstract void Collide(TDCollider collider);

    protected void CollideCylinderCylinder(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2)
    {
        if (cylinder1.CenterZLow < cylinder2.CenterZHigh && cylinder1.CenterZHigh > cylinder2.CenterZLow)
        {
            float intersection = cylinder1.Radius + cylinder2.Radius - Vector2.Distance(cylinder1.CenterXY, cylinder2.CenterXY);
            if (intersection > 0f)
            {
                if (cylinder1.IsTrigger || cylinder2.IsTrigger)
                {
                    cylinder1.IsColliding = true;
                    cylinder2.IsColliding = true;
                }
                else
                {
                    Vector3 direction = new Vector3(.5f * intersection * Vector2.Normalize(cylinder1.CenterXY - cylinder2.CenterXY), 0f);

                    cylinder1.TDObject.Transform.LocalPosition += direction;
                    cylinder2.TDObject.Transform.LocalPosition -= direction;
                }
            }
        }
    }

    protected void CollideCylinderCuboid(TDCylinderCollider cylinder, TDCuboidCollider cuboid)
    {
        if (cuboid.CuboidCornerLow.Z < cylinder.CenterZHigh && cuboid.CuboidCornerHigh.Z > cylinder.CenterZLow)
        {
            Vector2 closest = new Vector2(MathHelper.Clamp(cylinder.CenterXY.X, cuboid.CuboidCornerLow.X, cuboid.CuboidCornerHigh.X),
                MathHelper.Clamp(cylinder.CenterXY.Y, cuboid.CuboidCornerLow.Y, cuboid.CuboidCornerHigh.Y));

            float intersection = cylinder.Radius - Vector2.Distance(closest, cylinder.CenterXY);
            if (intersection > 0f)
            {
                if (cylinder.IsTrigger || cuboid.IsTrigger)
                {
                    cylinder.IsColliding = true;
                    cuboid.IsColliding = true;
                }
                else
                {
                    Vector3 direction = new Vector3(intersection * Vector2.Normalize(cylinder.CenterXY - closest), 0f);

                    cylinder.TDObject.Transform.LocalPosition += direction;
                }
            }
        }
    }
}
