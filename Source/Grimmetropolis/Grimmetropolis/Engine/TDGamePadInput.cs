using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDGamePadInput : TDInput
{
    /// <summary>
    /// The values refer to the XBox controller.
    /// 
    /// Xbox controller:
    ///      Y
    ///    X   B
    ///      A
    ///
    /// PS4 controller:
    ///      ^
    ///   []   O
    ///      X
    ///
    /// Switch controller
    ///      X
    ///    Y   A
    ///      B
    ///      
    /// </summary>
    public class GamePadConfig
    {
        // TODO: decide on button mappings.
        // TODO: should we be able to choose which joystick to use
        public Buttons UseItem = Buttons.B;
        public Buttons CycleNext = Buttons.RightTrigger;
        public Buttons CyclePrevious = Buttons.LeftTrigger;
        public Buttons SwapItem = Buttons.Y;

        // TODO: do we need this for keyboard?
        public Buttons SelectBuildingType = Buttons.LeftShoulder;
        public Buttons SpecialAbility = Buttons.A;
    }

    public int GamePadIndex { get; private set; }

    private GamePadConfig _config;
    private GamePadCapabilities _capabilities;
    private GamePadState _gamePad;

    public TDGamePadInput(int gamePadIndex, GamePadConfig config = null) 
    {
        // TODO: do we need to check if controller is a gamepad?
        _config = config ?? new GamePadConfig();

        GamePadIndex = gamePadIndex;

        UpdateDevice();
    }

    public Vector2 GetMoveDirection()
    {
        // TODO: what if only one of the two is available?
        // TODO: check for right ThumbStick?
        if (!_capabilities.HasLeftXThumbStick || !_capabilities.HasLeftYThumbStick) return Vector2.Zero;

        Vector2 direction = _gamePad.ThumbSticks.Left;
        if (direction.LengthSquared() > 1f) direction.Normalize();
        return direction;
    }

    public bool IsUseItemPressed()
    {
        // TODO: check capabilities doesn't work with config.
        return _gamePad.IsButtonDown(_config.UseItem);
    }

    public bool IsCycleNextItemPressed()
    {
        return _gamePad.IsButtonDown(_config.CycleNext);
    }

    public bool IsCyclePreviousItemPressed()
    {
        return _gamePad.IsButtonDown(_config.CyclePrevious);
    }

    public bool IsSwapItemPressed()
    {
        return _gamePad.IsButtonDown(_config.SwapItem);
    }


    public bool IsSelectBuildingTypePressed()
    {
        return _gamePad.IsButtonDown(_config.SelectBuildingType);
    }

    public bool IsSpecialAbilityPressed()
    {
        return _gamePad.IsButtonDown(_config.SpecialAbility);
    }

    public void UpdateDevice()
    {
        _capabilities = GamePad.GetCapabilities(GamePadIndex);
        _gamePad = GamePad.GetState(GamePadIndex);
    }

    public int GetSelectedBuildingIndex()
    {
        throw new NotImplementedException();
    }
}
