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
            MoveUp = Keys.I,                   // w
            MoveLeft = Keys.J,               // a
            MoveDown = Keys.K,               // s
            MoveRight = Keys.L,             // d

            Action = Keys.N,       // SPACE
            Cancel = Keys.M,
                 
            CycleNext = Keys.U,
            CyclePrevious = Keys.O,

            BuildMode = Keys.B

        };
        TDKeyboardInput keyboardInputArrow = new TDKeyboardInput(config);
        // Inputs.Add(keyboardInputArrow);

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
