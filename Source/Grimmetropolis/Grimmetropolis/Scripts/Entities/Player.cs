using Microsoft.Xna.Framework;
public class Player : Character
{
    public TDInput Input;

    public override void Initialize()
    {
        base.Initialize();

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

        Input = null;
        GameManager.Instance.Players.Remove(this);
    }
}
