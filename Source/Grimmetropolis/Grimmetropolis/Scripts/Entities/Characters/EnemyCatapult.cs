using Microsoft.Xna.Framework;

public class EnemyCatapult : Enemy
{
    public override string MeshName => "EnemyCatapult";

    public override EnemyMove.Type Actions => EnemyMove.Type.Run | EnemyMove.Type.RangedAttack;

    protected override void ShootProjectile(RangedAttackMove nextMove)
    {
        Animation.UseAnimation();

        if (Animation is CatapultAnimation catapultAnimation)
        {
            TDObject.RunAction(0f, (p) =>
            {
                // nextMove.Target.Health -= _damageAgainstBuildings;
                TDObject stonePayloadObject = PrefabFactory.CreatePrefab(PrefabType.StonePayload);
                Projectile stonePayload = stonePayloadObject.GetComponent<Projectile>();

                //TODO: if shot from enemy height, enemy hits itself ...
                stonePayload.StartPosition = TDObject.Transform.Position + new Vector3(0f, 0f, 1.25f);
                stonePayload.TargetCharacter = nextMove.Target;
                stonePayload.Damage = DamageAgainstBuildigns;
                stonePayload.Speed = ProjectileSpeed;
                stonePayload.IsEvilArrow = true;
            }, catapultAnimation.PartialArmUseTime);
        }
    }
}
