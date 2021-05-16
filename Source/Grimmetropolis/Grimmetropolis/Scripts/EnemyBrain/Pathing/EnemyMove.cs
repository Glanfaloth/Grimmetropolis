using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    [Flags]
    public enum Type
    {
        None            = 0x00,
        Run             = 0x01,
        Attack          = 0x02,
        RangedAttack    = 0x04,

        // special moves 
        PickUpArtifact      = 0x1000,
        StealArtifact       = 0x2000,
    }

    public abstract Type MovementType { get; }

    public Location From { get; }
    public Location To { get; }

    public abstract NextMoveInfo CreateInfo();

    public abstract float Cost { get; }

    public abstract bool ShouldPathBeRecomputed();

    protected EnemyMove(Location location)
    {
        From = location;
        To = location;
    }

    protected EnemyMove(Location from, Location to)
    {
        From = from;
        To = to;

        from.AddOutgoingEdge(this);
        to.AddIncomingEdge(this);
    }

    public virtual bool IsMoveAllowed(Type actions, float attackRange)
    {
        return (actions & MovementType) == MovementType;
    }
}