using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

public static class TDInputManager
{

    public static List<TDInput> Inputs = new List<TDInput>();
    public static TDInput DefaultInput;

    public static void Initialize()
    {
        TDKeyboardInput keyboardInput = new TDKeyboardInput();
        Inputs.Add(keyboardInput);

        TDKeyboardInput.KeyboardConfig config = new TDKeyboardInput.KeyboardConfig()
        {
            MoveUp = Keys.Up,                   // w
            MoveLeft = Keys.Left,               // a
            MoveDown = Keys.Down,               // s
            MoveRight = Keys.Right,             // d

            UseItem = Keys.NumPad0,             // SPACE
            UseItemAlt = Keys.Enter,            // NONE

            CycleNext = Keys.OemComma,          // Q
            CyclePrevious = Keys.OemPeriod,     // E
            SwapItem = Keys.RightShift,         // L-SHIFT

            SelectBuildingType = Keys.Back,     // TAB
            SpecialAbility = Keys.RightControl, // L-CTRL
        };
        TDKeyboardInput keyboardInputArrow = new TDKeyboardInput(config);
        Inputs.Add(keyboardInputArrow);

        TDInput firstGamePadInput = new TDGamePadInput(0);

        for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
        {
            TDInput inputDevice = new TDGamePadInput(i);
            Inputs.Add(inputDevice);
        }

        DefaultInput = new TDHybridInput(keyboardInput, firstGamePadInput);
    }

    public static void Update()
    {
        foreach (TDInput inputDevice in Inputs)
        {
            inputDevice.UpdateDevice();
        }

        DefaultInput.UpdateDevice();
    }
}
