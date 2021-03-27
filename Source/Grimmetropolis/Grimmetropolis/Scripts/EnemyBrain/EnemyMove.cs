using Microsoft.Xna.Framework;
using System;

public abstract class EnemyMove
{
    public enum Type
    {
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

        from.AddEdge(this);
        to.AddEdge(this);
    }
}