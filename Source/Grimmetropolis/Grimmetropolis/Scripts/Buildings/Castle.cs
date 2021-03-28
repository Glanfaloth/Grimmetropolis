using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override void Initialize()
    {
        Health = 9f;

        Size.X = 3;
        Size.Y = 3;

        base.Initialize();
    }
}
