using Microsoft.Xna.Framework;

using System;

public class Projectile : TDComponent
{
    public Vector3 StartPosition = Vector3.Zero;
    public Vector3 TargetPosition = Vector3.Zero;

    public Character TargetCharacter = null;

    public float Speed = 6f;
    public float Damage = 1f;

    private Vector3 _direction = Vector3.Zero;

    private float _distance = 0f;
    private float _time = 0f;

    public override void Initialize()
    {
        base.Initialize();

        _direction = TargetPosition - StartPosition;
        _distance = _direction.Length();

        float angle = MathF.Atan2(_direction.Y, _direction.X);

        TDObject.Transform.Position = StartPosition;
        TDObject.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, angle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Speed * _time >= _distance)
        {
            TDObject.Destroy();
            TargetCharacter.Health -= Damage;
        }
        else
        {
            TDObject.Transform.Position = Vector3.Lerp(StartPosition, TargetPosition, Speed * _time / _distance);
        }
    }
}
