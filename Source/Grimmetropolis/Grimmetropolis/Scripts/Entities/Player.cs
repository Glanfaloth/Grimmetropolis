using Microsoft.Xna.Framework;
public class Player : Character
{
    public TDInput Input;

    public override void Initialize()
    {
        base.Initialize();

        Input = null;
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 inputDirection = Input?.GetMoveDirection() ?? Vector2.Zero;
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);
    }
}
