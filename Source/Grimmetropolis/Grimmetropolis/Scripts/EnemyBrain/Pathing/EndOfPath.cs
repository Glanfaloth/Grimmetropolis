using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

class EndOfPath : EnemyMove
{
    public override Type MovementType => Type.EndOfPath;

    public override float Cost => 0;

    public EndOfPath(Location location) : base(location)
    {
    }

    public override NextMoveInfo CreateInfo()
    {
        return new NextMoveInfo(null, MovementType, Vector2.Zero);
    }
}
