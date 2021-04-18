using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


class EnemyWitch : Enemy
{

    public override void Initialize()
    {
        base.Initialize();

        TDObject.RunAction(4f, (p) =>
        {
            Vector3 position = TDObject.Transform.Position;
            position.Z = .5f + .25f * MathF.Sin(MathHelper.TwoPi * p);
            TDObject.Transform.Position = position;
        }, true);
    }

    public override string MeshName => "EnemyWitch";

    protected override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;

}
