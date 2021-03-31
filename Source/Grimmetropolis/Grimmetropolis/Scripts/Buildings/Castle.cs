using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override void Initialize()
    {
        Size.X = 3;
        Size.Y = 3;

        Health = 9f;

        base.Initialize();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
