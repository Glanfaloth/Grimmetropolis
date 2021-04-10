using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    private static Location _noLocation = new Location();
    public static EnemyMove NONE { get; } = new NoMove(_noLocation);

    [Flags]
    public enum Type
    {
        None            = 0x00,
        StealArtifact   = 0x01,
        Run             = 0x02,
        Attack          = 0x04,
        RangedAttack    = 0x08,
    }

    public abstract Type MovementType { get; }

    public Location From { get; }
    public Location To { get; }
    public abstract float Cost { get; }

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

    private class NoMove : EnemyMove
    {

        public override Type MovementType => Type.None;

        public override float Cost => 0;

        internal NoMove(Location location) : base(location)
        {
        }
    }
}