using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


class EnemyWitch : Enemy
{
    private float _zAlignment;

    public override void Initialize()
    {
        base.Initialize();

        _zAlignment = _controller.NextFloat() * MathHelper.Pi;
    }

    public override string MeshName => "EnemyWitch";

    protected override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _zAlignment += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // TODO: check if this doesn't break anything concerning collision detection
        // ideally, only the mesh should be offset
        Vector3 newPosition = TDObject.Transform.LocalPosition;
        newPosition.Z = (MathF.Sin(_zAlignment*3) + 1.2f) * .3f;
        TDObject.Transform.LocalPosition = newPosition;
    }
}
