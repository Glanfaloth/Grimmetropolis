using Microsoft.Xna.Framework;

using System;

public class EnemyWitch : Enemy
{

    public override void Initialize()
    {
        base.Initialize();

        TDObject.RunAction(2.5f, (p) =>
        {
            Vector3 position = TDObject.Transform.Position;
            position.Z = .5f + .375f * MathF.Sin(MathHelper.TwoPi * p);
            TDObject.Transform.Position = position;
        }, true);
    }

    public override string MeshName => "EnemyWitch";

    public override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;
    protected override void ShootProjectile(ITarget target)
    {
        // nextMove.Target.Health -= _damageAgainstBuildings;
        TDObject icicleObject = PrefabFactory.CreatePrefab(PrefabType.Icicle);
        Projectile icicle = icicleObject.GetComponent<Projectile>();

        //TODO: if shot from enemy height, enemy hits itself ...
        icicle.StartPosition = TDObject.Transform.Position + 1.25f * Vector3.Backward;
        icicle.TargetCharacter = target;
        icicle.Damage = DamageAgainstBuildigns;
        icicle.Speed = ProjectileSpeed;
        icicle.IsEvilArrow = true;
    }
}
