using Microsoft.Xna.Framework;
using System;

internal class RunMove : EnemyMove
{

    public override Type MovementType => Type.Run;

    public Vector2 Destination { get; }
    public RunMove(Location from, Location to, float cost, Vector2 destination) : base(from, to, cost)
    {
        Destination = destination;
    }
}
