using System;
using System.Collections.Generic;
using System.Text;


class EnemyKnight : Enemy
{
    public override string MeshName => "EnemyKnight";

    protected override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.Attack;
}
