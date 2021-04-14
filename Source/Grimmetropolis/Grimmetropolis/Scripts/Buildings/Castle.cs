﻿using Microsoft.Xna.Framework;

public class Castle : Building
{
    public override float BuildTime => 0;

    public override void Initialize()
    {
        Size.X = Config.CASTLE_SIZE_X;
        Size.Y = Config.CASTLE_SIZE_Y;

        BaseHealth = Config.CASTLE_HEALTH;
        Health = Config.CASTLE_HEALTH;

        // TODO: remove this work around
        // this needs to be passable since the ai tries to go to the castle location, otherwise no path will be found
        IsPassable = true;

        base.Initialize();
    }

    public override ResourcePile GetResourceCost()
    {
        return new ResourcePile(0, 0);
    }

    protected override void DoUpdate(GameTime gameTime)
    {
    }
}
