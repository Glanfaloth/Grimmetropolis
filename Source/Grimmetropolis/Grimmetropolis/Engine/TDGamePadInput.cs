using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDGamePadInput : TDInput
{
    public int GamePadIndex { get; private set; }

    private GamePadCapabilities _capabilities;
    private GamePadState _gamePad;

    private float _joystickThreshold;
    private float _triggerThreshold;

    public TDGamePadInput(int gamePadIndex) {

        GamePadIndex = gamePadIndex;

        UpdateDevice();

        _joystickThreshold = MathF.Pow(0.05f, 2f);
        _triggerThreshold = 0.95f;
    }

    public Vector2 GetMoveDirection()
    {
        if (!_capabilities.HasLeftStickButton) return Vector2.Zero;

        Vector2 direction = _gamePad.ThumbSticks.Left;
        if (direction.LengthSquared() > 1f) direction.Normalize();
        return direction.LengthSquared() > _joystickThreshold ? direction : Vector2.Zero;
    }

    public bool IsActionButtonPressed()
    {
        if (!_capabilities.HasAButton) return false;

        return _gamePad.Buttons.A == ButtonState.Pressed;
    }

    public bool IsCycleNextItemPressed()
    {
        if (!_capabilities.HasBButton) return false;

        return _gamePad.Buttons.B == ButtonState.Pressed;
    }

    public bool IsSelectBuildingTypePressed()
    {
        if (!_capabilities.HasLeftShoulderButton) return false;

        return _gamePad.Buttons.LeftShoulder == ButtonState.Pressed;
    }

    public bool IsSpecialAbilityPressed()
    {
        if (!_capabilities.HasLeftTrigger) return false;

        return _gamePad.Triggers.Left > _triggerThreshold;
    }

    public void UpdateDevice()
    {
        _capabilities = GamePad.GetCapabilities(GamePadIndex);
        _gamePad = GamePad.GetState(GamePadIndex);
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
