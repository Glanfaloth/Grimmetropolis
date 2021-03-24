using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDGamePadInput : TDInput
{
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

    public override Vector2 J1Direction()
    {
        if (!_capabilities.HasLeftStickButton) return Vector2.Zero;

        Vector2 direction = _gamePad.ThumbSticks.Left;
        if (direction.LengthSquared() > 1f) direction.Normalize();
        return direction.LengthSquared() > _joystickThreshold ? direction : Vector2.Zero;
    }

    public override bool APressed()
    {
        if (!_capabilities.HasAButton) return false;

        return _gamePad.Buttons.A == ButtonState.Pressed;
    }

    public override bool BPressed()
    {
        if (!_capabilities.HasBButton) return false;

        return _gamePad.Buttons.B == ButtonState.Pressed;
    }

    public override bool L1Pressed()
    {
        if (!_capabilities.HasLeftShoulderButton) return false;

        return _gamePad.Buttons.LeftShoulder == ButtonState.Pressed;
    }

    public override bool L2Pressed()
    {
        if (!_capabilities.HasLeftTrigger) return false;

        return _gamePad.Triggers.Left > _triggerThreshold;
    }

    public override void UpdateDevice()
    {
        _capabilities = GamePad.GetCapabilities(GamePadIndex);
        _gamePad = GamePad.GetState(GamePadIndex);
    }
}
