using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

class EndOfPath : EnemyMove
{
    public override Type MovementType { get; }

    public override float Cost => 0;

    public EndOfPath(Location location, Type endOfPathAction) : base(location)
    {
        MovementType = endOfPathAction;
    }

    public override NextMoveInfo CreateInfo()
    {
        return new NextMoveInfo(null, MovementType, Vector2.Zero);
    }
}
