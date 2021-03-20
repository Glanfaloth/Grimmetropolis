using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

public static class TDInputManager
{

    public static List<TDInput> Inputs = new List<TDInput>();

    private static TDKeyboardInput _keyboardInput;
    private static TDGamePadInput[] _gamePadInputs = new TDGamePadInput[GamePad.MaximumGamePadCount];

    public static void Initialize()
    {
        _keyboardInput = new TDKeyboardInput();
        Inputs.Add(_keyboardInput);

        for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
        {
            _gamePadInputs[i] = new TDGamePadInput(i);
            Inputs.Add(_gamePadInputs[i]);
        }
    }

    public static void Update()
    {
        _keyboardInput.UpdateDevice();

        foreach (TDGamePadInput gamePadInput in _gamePadInputs)
        {
            gamePadInput.UpdateDevice();
        }
    }
}
