using Microsoft.Xna.Framework;

public abstract class TDInput
{
    public int GamePadIndex { get; set; }

    public abstract Vector2 J1Direction();
    public abstract bool APressed();
    public abstract bool BPressed();
    public abstract bool L1Pressed();
    public abstract bool L2Pressed();

    public abstract void UpdateDevice();
}
