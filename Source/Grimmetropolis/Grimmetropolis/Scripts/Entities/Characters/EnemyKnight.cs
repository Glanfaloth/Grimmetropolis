using System;
using System.Collections.Generic;
using System.Text;


public class EnemyKnight : Enemy
{
    public override string MeshName => "EnemyKnight";

    public override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.Attack;
}
