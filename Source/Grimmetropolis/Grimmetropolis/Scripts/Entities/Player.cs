using Microsoft.Xna.Framework;
public class Player : Character
{
    private TDInput _input;

    public Player(TDObject tdObject, float lookingAngle) : base(tdObject, lookingAngle)
    {
        _input = TDInputManager.Inputs[0];
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 inputDirection = _input.GetMoveDirection();
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);
    }
}
