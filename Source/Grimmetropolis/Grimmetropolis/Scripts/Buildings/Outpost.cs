using Microsoft.Xna.Framework;

public class Outpost : Building
{
    public override void Initialize()
    {
        WoodCost = 1f;
        StoneCost = 1f;

        base.Initialize();
    }
}
