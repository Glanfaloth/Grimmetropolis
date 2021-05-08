using Microsoft.Xna.Framework;
using System;

internal class RunMove : EnemyMove
{
    private readonly float _distanceCost;

    public override Type MovementType => Type.Run;

    public MapTile Destination { get; }

    public override float Cost
    {
        get
        {
            return _distanceCost * Config.RUN_MOVE_DISTANCE_FACTOR * (1 + Destination.NearbyOutposts * Config.RUN_MOVE_OUTPOST_COUNT_SCALE_FACTOR);
        }
    }

    public RunMove(Location from, Location to, float distanceCost, MapTile destination) : base(from, to)
    {
        Destination = destination;
        _distanceCost = distanceCost;
    }

    public override NextMoveInfo CreateInfo()
    {
        return new NextMoveInfo(null, MovementType, To.Tile.TDObject.Transform.LocalPosition.GetXY());
    }
}
