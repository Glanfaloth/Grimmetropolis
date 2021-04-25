using System;
using System.Collections.Generic;
using System.Text;


class EnemyCatapult : Enemy
{
    public override string MeshName => "EnemyCatapult";

    protected override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;
}
