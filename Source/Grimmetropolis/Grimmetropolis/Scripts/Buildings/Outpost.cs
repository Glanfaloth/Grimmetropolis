using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override void Initialize()
    {
        base.Initialize();

        Size.X = 3;
        Size.Y = 3;
    }
}
