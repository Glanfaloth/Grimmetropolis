using System;
using System.Collections.Generic;
using System.Text;

class StealArtifact : EnemyMove
{
    public override Type MovementType => Type.StealArtifact;

    public override float Cost => 0;

    public StealArtifact(Location from, Location to) : base(from, to)
    {
    }
}
