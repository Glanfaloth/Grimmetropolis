using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    private static Location _noLocation = new Location();
    public static EnemyMove NONE { get; } = new NoMove(_noLocation, _noLocation, 0);

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
    public float Cost { get; }

    protected EnemyMove(Location from, Location to, float cost)
    {
        From = from;
        To = to;
        Cost = cost;

        from.AddOutgoingEdge(this);
        to.AddIncomingEdge(this);
    }

    private class NoMove : EnemyMove
    {

        public override Type MovementType => Type.None;

        internal NoMove(Location from, Location to, float cost) : base(from, to, cost)
        {
        }
    }
}