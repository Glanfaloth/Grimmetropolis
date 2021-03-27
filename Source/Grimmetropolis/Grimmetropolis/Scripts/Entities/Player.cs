using Microsoft.Xna.Framework;
public class Player : Character
{
    public TDInput Input;

    public override void Initialize()
    {
        base.Initialize();

        Input = null;

        GameManager.Instance.Players.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 inputDirection = Input?.GetMoveDirection() ?? Vector2.Zero;
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        GameManager.Instance.Players.Remove(this);
    }
}
