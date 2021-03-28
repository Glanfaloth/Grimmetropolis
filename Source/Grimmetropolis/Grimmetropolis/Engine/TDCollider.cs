﻿using Microsoft.Xna.Framework;

using System.Diagnostics;

public delegate void CollisionCylinderCylinderEvent(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2, float intersection);
public delegate void CollisionCylidnerCuboidEvent(TDCylinderCollider cylinder, TDCuboidCollider cuboid, Vector2 closest, float intersection);

public abstract class TDCollider : TDComponent
{
    public bool IsColliding = false;
    public bool IsTrigger = false;

    public CollisionCylinderCylinderEvent collisionCylinderCylinderEvent;
    public CollisionCylidnerCuboidEvent collisionCylinderCuboidEvent;

    public override void Initialize()
    {
        base.Initialize();

        TDSceneManager.ActiveScene.ColliderObjects.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.ColliderObjects.Remove(this);
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
                collisionCylinderCylinderEvent?.Invoke(cylinder1, cylinder2, intersection);
                cylinder2.collisionCylinderCylinderEvent?.Invoke(cylinder1, cylinder2, intersection);

                if (cylinder1.IsTrigger || cylinder2.IsTrigger)
                {
                    cylinder1.IsColliding = true;
                    cylinder2.IsColliding = true;
                }
                else
                {
                    Vector2 collidingDirection = Vector2.Normalize(cylinder1.CenterXY - cylinder2.CenterXY);
                    if (float.IsNaN(collidingDirection.X)) collidingDirection = new Vector2(1f, 0f);

                    Vector3 direction = new Vector3(.5f * intersection * collidingDirection, 0f);

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
                collisionCylinderCuboidEvent?.Invoke(cylinder, cuboid, closest, intersection);
                cuboid.collisionCylinderCuboidEvent?.Invoke(cylinder, cuboid, closest, intersection);

                if (cylinder.IsTrigger || cuboid.IsTrigger)
                {
                    cylinder.IsColliding = true;
                    cuboid.IsColliding = true;
                }
                else
                {
                    Vector2 collidingDirection = Vector2.Normalize(cylinder.CenterXY - closest);
                    if (float.IsNaN(collidingDirection.X)) collidingDirection = new Vector2(1f, 0f);

                    Vector3 direction = new Vector3(intersection * collidingDirection, 0f);
                    cylinder.TDObject.Transform.LocalPosition += direction;
                }
            }
        }
    }
}
