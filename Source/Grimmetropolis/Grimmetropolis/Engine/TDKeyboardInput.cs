using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDKeyboardInput : TDInput
{
    private KeyboardState _keyboard;

    public TDKeyboardInput() {
        UpdateDevice();
    }

    public override Vector2 J1Direction()
    {
        Vector2 direction = Vector2.Zero;
        if (_keyboard.IsKeyDown(Keys.D)) direction.X += 1f;
        if (_keyboard.IsKeyDown(Keys.A)) direction.X -= 1f;
        if (_keyboard.IsKeyDown(Keys.W)) direction.Y += 1f;
        if (_keyboard.IsKeyDown(Keys.S)) direction.Y -= 1f;
        if (direction.LengthSquared() > 1f) direction.Normalize();

        return direction;
    }

    public override bool APressed()
    {
        return _keyboard.IsKeyDown(Keys.Space) || _keyboard.IsKeyDown(Keys.Enter);
    }

    public override bool BPressed()
    {
        return _keyboard.IsKeyDown(Keys.Back);
    }

    public override bool L1Pressed()
    {
        return _keyboard.IsKeyDown(Keys.LeftShift);
    }

    public override bool L2Pressed()
    {
        return _keyboard.IsKeyDown(Keys.LeftControl);
    }

    public override void UpdateDevice()
    {
        base.UpdateDevice();

        _keyboard = Keyboard.GetState();
    }
}
