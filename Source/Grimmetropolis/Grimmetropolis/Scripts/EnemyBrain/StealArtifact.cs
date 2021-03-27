using System;
using System.Collections.Generic;
using System.Text;

class StealArtifact : EnemyMove
{
    public override Type MovementType => Type.StealArtifact;

    public StealArtifact(Location from, Location to, float cost) : base(from, to, cost)
    {
    }
}
