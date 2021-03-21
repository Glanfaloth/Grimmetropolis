using Microsoft.Xna.Framework;

using System.Diagnostics;

public class MoveComponent : TDComponent
{

    private float _speedMovement;

    private TDCollider _collider;
    public MoveComponent(TDObject tdObject, TDCollider collider) : base(tdObject)
    {
        _speedMovement = 4f;
        _collider = collider;
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
        }

        TDObject.Transform.LocalPosition += movement;

        Debug.WriteLine(_collider.IsColliding);
    }
}
