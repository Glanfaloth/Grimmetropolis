using Microsoft.Xna.Framework;

public interface TDInput
{
    // TODO: double check what actions are supported.
    public abstract Vector2 GetMoveDirection();

    // TODO: if we only support two items, maybe we should have button for each item individualy?
    public abstract bool IsUseItemPressed();
    public abstract bool IsCycleNextItemPressed();
    public abstract bool IsCyclePreviousItemPressed();
    public abstract bool IsSwapItemPressed();

    // TODO: decide how selecting the build will be done
    public abstract bool IsSelectBuildingTypePressed();
    public abstract int GetSelectedBuildingIndex();

    public abstract bool IsSpecialAbilityPressed();

    public abstract void UpdateDevice();

    // TODO: how will we do aiming? Shoot straight or aim at target?
    // TODO: menu navigation?
}
