using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override void Initialize()
    {
        Size.X = 3;
        Size.Y = 3;

        Health = 9f;

        // TODO: remove this work around
        // this needs to be passable since the ai tries to go to the castle location, otherwise no path will be found
        IsPassable = true;

        base.Initialize();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
