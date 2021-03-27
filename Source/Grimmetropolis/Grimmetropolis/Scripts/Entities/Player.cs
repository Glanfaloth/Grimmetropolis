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
        Vector2 inputDirection = Input.GetMoveDirection();
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);

        if (Input.IsSpecialAbilityPressed()) Attack();
        if (Input.IsUseItemPressed()) Build();

        base.Update(gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        Input = null;
        GameManager.Instance.Players.Remove(this);
    }
}
