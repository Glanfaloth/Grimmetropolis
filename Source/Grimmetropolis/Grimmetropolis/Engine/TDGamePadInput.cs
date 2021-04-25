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
        public Buttons Action = Buttons.A;
        public Buttons Cancel = Buttons.B;

        public Buttons CycleNext = Buttons.DPadRight;
        public Buttons CyclePrevious = Buttons.DPadLeft;

        public Buttons BuildMode = Buttons.RightShoulder;
    }

    public int GamePadIndex { get; private set; }

    private GamePadConfig _config;
    private GamePadCapabilities _capabilities;
    private GamePadState _gamePad;

    public TDGamePadInput(int gamePadIndex, GamePadConfig config = null) 
    {
        _config = config ?? new GamePadConfig();

        GamePadIndex = gamePadIndex;

        UpdateDevice();
    }

    public Vector2 MoveDirection()
    {
        // TODO: what if only one of the two is available?
        // TODO: check for right ThumbStick?
        if (!_capabilities.HasLeftXThumbStick || !_capabilities.HasLeftYThumbStick) return Vector2.Zero;

        Vector2 direction = _gamePad.ThumbSticks.Left;
        if (direction.LengthSquared() > 1f) direction.Normalize();
        return direction;
    }

    public bool ActionPressed()
    {
        // TODO: check capabilities doesn't work with config.
        return _gamePad.IsButtonDown(_config.Action);
    }

    public bool CancelPressed()
    {
        return _gamePad.IsButtonDown(_config.Cancel);
    }

    public bool CycleRightPressed()
    {
        return _gamePad.IsButtonDown(_config.CycleNext);
    }

    public bool CycleLeftPressed()
    {
        return _gamePad.IsButtonDown(_config.CyclePrevious);
    }

    public bool BuildModePressed()
    {
        return _gamePad.IsButtonDown(_config.BuildMode);
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
