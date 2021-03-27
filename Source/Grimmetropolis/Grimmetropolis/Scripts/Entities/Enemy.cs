using Microsoft.Xna.Framework;
public class Enemy : Character
{

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 direction = -new Vector2(TDObject.Transform.LocalPosition.X, TDObject.Transform.LocalPosition.Y);
        if (direction.LengthSquared() > 1f) direction.Normalize();

        Move(direction, gameTime);
    }
}
