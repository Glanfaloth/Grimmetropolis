using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

public class TDHybridInput : TDInput
{
    private readonly TDInput _inputOne;
    private readonly TDInput _inputTwo;

    public TDHybridInput(TDInput inputOne, TDInput inputTwo)
    {
        _inputOne = inputOne;
        _inputTwo = inputTwo;
    }

    public Vector2 MoveDirection()
    {
        Vector2 direction = _inputOne.MoveDirection() + _inputTwo.MoveDirection();
        if (direction.LengthSquared() > 1) direction.Normalize();
        return direction;
    }

    public bool ActionPressed()
    {
        return _inputOne.ActionPressed()
            || _inputTwo.ActionPressed();
    }

    public bool CancelPressed()
    {
        return _inputOne.CancelPressed()
            || _inputTwo.CancelPressed();
    }

    public bool CycleRightPressed()
    {
        return _inputOne.CycleRightPressed()
            || _inputTwo.CycleRightPressed();
    }

    public bool CycleLeftPressed()
    {
        return _inputOne.CycleLeftPressed()
            || _inputTwo.CycleLeftPressed();
    }

    public bool BuildModePressed()
    {
        return _inputOne.BuildModePressed()
            || _inputTwo.BuildModePressed();
    }

    public void UpdateDevice()
    {
        _inputOne.UpdateDevice();
        _inputTwo.UpdateDevice();
    }
}
