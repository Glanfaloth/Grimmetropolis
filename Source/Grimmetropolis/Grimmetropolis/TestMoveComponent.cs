using Microsoft.Xna.Framework;

using System.Diagnostics;

public class TestMoveComponent : TDComponent
{

    private float _speedMovement;
    private TDCollider _collider;

    public TestMoveComponent(TDObject tdObject) : base(tdObject)
    {
        _speedMovement = 4f;
        _collider = TDObject.GetComponent<TDCollider>();
        _collider.collisionCylinderCylinderEvent += ReactToCylinderCollision;
        _collider.collisionCylinderCuboidEvent += ReactToCuboidCollision;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Vector3 movement = Vector3.Zero;
        foreach (TDInput input in TDInputManager.Inputs)
        {
            Vector2 j1Direction = input.J1Direction();
            movement.X -= j1Direction.Y * _speedMovement * (float)gameTime.ElapsedGameTime.TotalSeconds;
            movement.Y += j1Direction.X * _speedMovement * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.L1Pressed()) movement.Z += _speedMovement * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.L2Pressed()) movement.Z -= _speedMovement * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        TDObject.Transform.LocalPosition += movement;
    }

    public void ReactToCylinderCollision(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2, float intersection)
    {
        Debug.WriteLine("I hit a cylinder.");
    }

    public void ReactToCuboidCollision(TDCylinderCollider cylinder, TDCuboidCollider cuboid, Vector2 closest, float intersection)
    {
        Debug.WriteLine("I hit a cube.");
    }
}
