using Microsoft.Xna.Framework;

public interface TDInput
{
    // TODO: double check what actions are supported.
    public abstract Vector2 MoveDirection();

    // TODO: if we only support two items, maybe we should have button for each item individualy?
    public abstract bool ActionPressed();
    public abstract bool CancelPressed();
    public abstract bool CycleRightPressed();
    public abstract bool CycleLeftPressed();

    // TODO: decide how selecting the build will be done
    // depending on input type, we may consider a different UI.
    public abstract bool BuildModePressed();

    public abstract void UpdateDevice();

    // TODO: how will we do aiming? Shoot straight or aim at target?
    // TODO: menu navigation?
}
