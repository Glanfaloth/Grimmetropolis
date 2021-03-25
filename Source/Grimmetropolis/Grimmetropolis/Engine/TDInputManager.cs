using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

public static class TDInputManager
{

    public static List<TDInput> Inputs = new List<TDInput>();

    public static void Initialize()
    {
        TDKeyboardInput keyboardInput = new TDKeyboardInput();
        TDGamePadInput firstGamePadInput = new TDGamePadInput(0);
        TDHybridInput defaultInput = new TDHybridInput(keyboardInput, firstGamePadInput);

        Inputs.Add(defaultInput);

        for (int i = 1; i < GamePad.MaximumGamePadCount; i++)
        {
            TDInput inputDevice = new TDGamePadInput(i);
            Inputs.Add(inputDevice);
        }
    }

    public static void Update()
    {
        foreach (TDInput inputDevice in Inputs)
        {
            inputDevice.UpdateDevice();
        }
    }
}
