using System;
using System.Collections.Generic;
using System.Text;


class EnemyWitch : Enemy
{
    public override string MeshName => "EnemyWitch";

    protected override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;
}
