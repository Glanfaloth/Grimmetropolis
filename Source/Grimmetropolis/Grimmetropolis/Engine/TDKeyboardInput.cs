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

        public Keys UseItem     = Keys.Space;
        public Keys UseItemAlt  = Keys.Enter;

        public Keys CycleNext     = Keys.Q;
        public Keys CyclePrevious = Keys.E;
        public Keys SwapItem = Keys.LeftShift;

        // TODO: do we need this for keyboard?
        public Keys SelectBuildingType = Keys.Tab;
        public Keys SpecialAbility = Keys.LeftControl;
    }

    private KeyboardState _keyboard;
    private readonly KeyboardConfig _config;

    public TDKeyboardInput(KeyboardConfig config = null) {
        _config = config ?? new KeyboardConfig();
        UpdateDevice();
    }

    public Vector2 GetMoveDirection()
    {
        Vector2 direction = Vector2.Zero;
        if (_keyboard.IsKeyDown(_config.MoveRight)) direction.X += 1f;
        if (_keyboard.IsKeyDown(_config.MoveLeft)) direction.X -= 1f;
        if (_keyboard.IsKeyDown(_config.MoveUp)) direction.Y += 1f;
        if (_keyboard.IsKeyDown(_config.MoveDown)) direction.Y -= 1f;
        if (direction.LengthSquared() > 1f) direction.Normalize();

        return direction;
    }

    public bool IsUseItemPressed()
    {
        return _keyboard.IsKeyDown(_config.UseItem) || _keyboard.IsKeyDown(_config.UseItemAlt);
    }

    public bool IsCycleNextItemPressed()
    {
        return _keyboard.IsKeyDown(_config.CycleNext);
    }

    public bool IsCyclePreviousItemPressed()
    {
        return _keyboard.IsKeyDown(_config.CyclePrevious);
    }

    public bool IsSwapItemPressed()
    {
        return _keyboard.IsKeyDown(_config.SwapItem);
    }


    public bool IsSelectBuildingTypePressed()
    {
        return _keyboard.IsKeyDown(_config.SelectBuildingType);
    }

    public int GetSelectedBuildingIndex()
    {
        throw new NotImplementedException();
    }


    public bool IsSpecialAbilityPressed()
    {
        return _keyboard.IsKeyDown(_config.SpecialAbility);
    }

    public void UpdateDevice()
    {
        _keyboard = Keyboard.GetState();
    }
}
