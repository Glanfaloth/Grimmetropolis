using System;
using System.Collections.Generic;
using System.Text;

class TakeArtifact : EnemyMove
{
    public override Type MovementType => Type.TakeArtifact;

    public override float Cost => 0;

    public TakeArtifact(Location location) : base(location)
    {
    }
}
