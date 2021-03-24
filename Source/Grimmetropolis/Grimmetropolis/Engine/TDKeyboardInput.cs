using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDKeyboardInput : TDInput
{
    private KeyboardState _keyboard;

    public TDKeyboardInput() {
        UpdateDevice();
    }

    public Vector2 GetMoveDirection()
    {
        Vector2 direction = Vector2.Zero;
        if (_keyboard.IsKeyDown(Keys.D)) direction.X += 1f;
        if (_keyboard.IsKeyDown(Keys.A)) direction.X -= 1f;
        if (_keyboard.IsKeyDown(Keys.W)) direction.Y += 1f;
        if (_keyboard.IsKeyDown(Keys.S)) direction.Y -= 1f;
        if (direction.LengthSquared() > 1f) direction.Normalize();

        return direction;
    }

    public bool IsActionButtonPressed()
    {
        return _keyboard.IsKeyDown(Keys.Space) || _keyboard.IsKeyDown(Keys.Enter);
    }

    public bool IsCycleNextItemPressed()
    {
        return _keyboard.IsKeyDown(Keys.Back);
    }

    public bool IsSelectBuildingTypePressed()
    {
        return _keyboard.IsKeyDown(Keys.LeftShift);
    }

    public bool IsSpecialAbilityPressed()
    {
        return _keyboard.IsKeyDown(Keys.LeftControl);
    }

    public void UpdateDevice()
    {
        _keyboard = Keyboard.GetState();
    }

    public bool IsUseItemPressed()
    {
        throw new NotImplementedException();
    }

    public bool IsCyclePreviousItemPressed()
    {
        throw new NotImplementedException();
    }

    public bool IsSwapItemPressed()
    {
        throw new NotImplementedException();
    }

    public int GetSelectedBuildingIndex()
    {
        throw new NotImplementedException();
    }
}
