using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    private static Location _noLocation = new Location(null);
    public static EnemyMove NONE { get; } = new NoMove(_noLocation);

    [Flags]
    public enum Type
    {
        None            = 0x00,
        Run             = 0x01,
        Attack          = 0x02,
        RangedAttack    = 0x04,

        // special moves 
        EndOfPath       = 0x1000,
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