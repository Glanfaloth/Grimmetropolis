using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


public class NextMoveInfo
{
    public ITarget Target { get; }

    public EnemyMove.Type MovementType { get; }

    public Vector2 LocalPosition { get; }

    public NextMoveInfo(ITarget target, EnemyMove.Type movementType, Vector2 localPosition)
    {
        Target = target;
        MovementType = movementType;
        LocalPosition = localPosition;
    }
}

