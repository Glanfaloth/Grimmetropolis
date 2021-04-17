using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDKeyboardInput : TDInput
{
    public class KeyboardConfig
    {
        // TODO: decide on key mappings.
        // TODO: alternative options for all keys?

        public Keys MoveUp      = Keys.W;
        public Keys MoveLeft    = Keys.A;
        public Keys MoveDown    = Keys.S;
        public Keys MoveRight   = Keys.D;

        public Keys Action     = Keys.Space;
        public Keys Cancel      = Keys.LeftAlt;

        public Keys CycleNext     = Keys.Q;
        public Keys CyclePrevious = Keys.E;

        public Keys BuildMode = Keys.Tab;
    }

    private KeyboardState _keyboard;
    private readonly KeyboardConfig _config;

    public TDKeyboardInput(KeyboardConfig config = null) {
        _config = config ?? new KeyboardConfig();
        UpdateDevice();
    }

    public Vector2 MoveDirection()
    {
        Vector2 direction = Vector2.Zero;
        if (_keyboard.IsKeyDown(_config.MoveRight)) direction.X += 1f;
        if (_keyboard.IsKeyDown(_config.MoveLeft)) direction.X -= 1f;
        if (_keyboard.IsKeyDown(_config.MoveUp)) direction.Y += 1f;
        if (_keyboard.IsKeyDown(_config.MoveDown)) direction.Y -= 1f;
        if (direction.LengthSquared() > 1f) direction.Normalize();

        return direction;
    }

    public bool ActionPressed()
    {
        return _keyboard.IsKeyDown(_config.Action);
    }

    public bool CancelPressed()
    {
        return _keyboard.IsKeyDown(_config.Cancel);
    }

    public bool CycleRightPressed()
    {
        return _keyboard.IsKeyDown(_config.CycleNext);
    }

    public bool CycleLeftPressed()
    {
        return _keyboard.IsKeyDown(_config.CyclePrevious);
    }

    public bool BuildModePressed()
    {
        return _keyboard.IsKeyDown(_config.BuildMode);
    }

    public void UpdateDevice()
    {
        _keyboard = Keyboard.GetState();
    }
}
