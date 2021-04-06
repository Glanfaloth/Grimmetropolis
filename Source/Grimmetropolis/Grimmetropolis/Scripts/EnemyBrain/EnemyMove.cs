using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    private static Location _noLocation = new Location();
    public static EnemyMove NONE { get; } = new NoMove(_noLocation, _noLocation);

    public enum Type
    {
        None,
        StealArtifact,
        Run,
        Attack,
    }

    public abstract Type MovementType { get; }

    public Location From { get; }
    public Location To { get; }
    public abstract float Cost { get; }

    protected EnemyMove(Location from, Location to)
    {
        From = from;
        To = to;

        from.AddOutgoingEdge(this);
        to.AddIncomingEdge(this);
    }

    private class NoMove : EnemyMove
    {

        public override Type MovementType => Type.None;

        public override float Cost => 0;

        internal NoMove(Location from, Location to) : base(from, to)
        {
        }
    }
}