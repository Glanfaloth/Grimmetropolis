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

        TDInput firstGamePadInput = new TDGamePadInput(0);

        for (int i = 1; i < GamePad.MaximumGamePadCount; i++)
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
