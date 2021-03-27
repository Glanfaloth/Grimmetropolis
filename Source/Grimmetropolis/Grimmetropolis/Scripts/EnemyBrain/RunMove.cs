using Microsoft.Xna.Framework;
using System;

internal class RunMove : EnemyMove
{

    public override Type MovementType => Type.Run;

    public Vector2 Destination { get; }
    public RunMove(Location from, Location to, float cost, Vector3 destination) : base(from, to, cost)
    {
        Destination = new Vector2(destination.X, destination.Y);
    }
}
