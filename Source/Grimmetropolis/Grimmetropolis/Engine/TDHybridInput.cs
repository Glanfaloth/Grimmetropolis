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

    public Vector2 GetMoveDirection()
    {
        Vector2 direction = _inputOne.GetMoveDirection() + _inputTwo.GetMoveDirection();
        if (direction.LengthSquared() > 1) direction.Normalize();
        return direction;
    }

    public int GetSelectedBuildingIndex()
    {
        // TODO: check if this is still correct.
        return Math.Max(_inputOne.GetSelectedBuildingIndex(), _inputTwo.GetSelectedBuildingIndex());
    }

    public bool IsCycleNextItemPressed()
    {
        return _inputOne.IsCycleNextItemPressed()
            || _inputTwo.IsCycleNextItemPressed();
    }

    public bool IsCyclePreviousItemPressed()
    {
        return _inputOne.IsCyclePreviousItemPressed()
            || _inputTwo.IsCyclePreviousItemPressed();
    }

    public bool IsSelectBuildingTypePressed()
    {
        return _inputOne.IsSelectBuildingTypePressed()
            || _inputTwo.IsSelectBuildingTypePressed();
    }

    public bool IsSpecialAbilityPressed()
    {
        return _inputOne.IsSpecialAbilityPressed()
            || _inputTwo.IsSpecialAbilityPressed();
    }

    public bool IsSwapItemPressed()
    {
        return _inputOne.IsSwapItemPressed()
            || _inputTwo.IsSwapItemPressed();
    }

    public bool IsUseItemPressed()
    {
        return _inputOne.IsUseItemPressed()
            || _inputTwo.IsUseItemPressed();
    }

    public void UpdateDevice()
    {
        _inputOne.UpdateDevice();
        _inputTwo.UpdateDevice();
    }
}
