using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class Enemy : Character
{
    protected EnemyController _controller;
    private float _walkSpeed;
    private float _rotateSpeed;
    private float _baseHealth;
    private float _damageAgainstPlayers;
    private float _damageAgainstBuildings;
    private float _attackRange;
    private float _attackRangeSquared;
    private float _attackDuration;
    private float _projectileSpeed;

    public abstract EnemyMove.Type Actions { get; }

    public abstract String MeshName { get; }

    public virtual String TextureName => "ColorPaletteTexture";

    public override float WalkSpeed => _walkSpeed;

    protected override float RotateSpeed => _rotateSpeed;

    public override float BaseHealth => _baseHealth;

    public float AttackRange => _attackRange;
    public float AttackDuration => _attackDuration;
    protected float ProjectileSpeed => _projectileSpeed;

    protected float DamageAgainstPlayers => _damageAgainstPlayers;
    protected float DamageAgainstBuildigns => _damageAgainstBuildings;

    public EnemyCommand CurrentCommand { get; set; }

    public override Vector3 OffsetTarget { get; } = .5f * Vector3.Backward;

    public Vector2 Position => TDObject?.Transform.LocalPosition.GetXY() ?? Vector2.Zero;

    public void SetBaseStats(Config.EnemyStats stats)
    {
        _walkSpeed = stats.WALK_SPEED;
        _rotateSpeed = stats.ROTATE_SPEED;
        _baseHealth = stats.HEALTH;
        _damageAgainstPlayers = stats.DAMAGE_AGAINST_PLAYER;
        _damageAgainstBuildings = stats.DAMAGE_AGAINST_BUILDINGS;
        _attackRange = stats.ATTACK_RANGE;
        _attackRangeSquared = _attackRange * _attackRange;
        _attackDuration = stats.ATTACK_DURATION;
        _projectileSpeed = stats.PROJECTILE_SPEED;
    }

    public override void Initialize()
    {
        base.Initialize();

        _controller = GameManager.Instance.EnemyController;

        GameManager.Instance.Enemies.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (CurrentCommand == null) return;

        NextMoveInfo nextMove = CurrentCommand.GetNextMoveInfo(TDObject.Transform.LocalPosition.GetXY(), Actions, _attackRange);


        CurrentWalkSpeed = 0f;

        switch (nextMove.MovementType)
        {
            case EnemyMove.Type.None:
                Debug.WriteLine("ERROR: no valid move found for enemy");
                break;
            case EnemyMove.Type.EndOfPath:
                // end of path means the enemy arrived at it's destination and has nothing to do

                // TODO: implement win condition
                if (Items[0] is MagicalArtifact)
                {
                    Debug.WriteLine("YOU LOSE");
                }
                break;
            case EnemyMove.Type.Run:
                MoveToTarget(nextMove.LocalPosition, gameTime);
                break;
            case EnemyMove.Type.Attack:
                AttackTarget(nextMove.Target, gameTime);
                break;
            case EnemyMove.Type.RangedAttack:
                RangedAttackTarget(nextMove.Target, gameTime);
                break;
            default:
                throw new NotSupportedException();
        }

        base.Update(gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        GameManager.Instance.Enemies.Remove(this);
    }
    protected override void Interact(GameTime gameTime)
    {
        base.Interact(gameTime);


        if (Cooldown <= 0f)
        {
            float closestPlayerDistance = float.MaxValue;
            float closestBuildingDistance = float.MaxValue;
            Player closestPlayer = null;
            Building closestBuilding = null;
            foreach (Tuple<TDCollider, float> colliderEntry in _colliderList)
            {
                if (colliderEntry.Item1 is TDCylinderCollider && closestPlayerDistance > colliderEntry.Item2)
                {
                    Player player = colliderEntry.Item1.TDObject?.GetComponent<Player>();
                    if (player != null)
                    {
                        closestPlayerDistance = colliderEntry.Item2;
                        closestPlayer = player;
                    }
                }
                else if (colliderEntry.Item1 is TDCuboidCollider && closestBuildingDistance > colliderEntry.Item2)
                {
                    if (colliderEntry.Item1.TDObject?.GetComponent<MapTile>().Structure is Building building)
                    {
                        closestBuildingDistance = colliderEntry.Item2;
                        closestBuilding = building;
                    }
                }
            }

            if (closestPlayer != null)
            {
                closestPlayer.Health -= _damageAgainstPlayers;
                Cooldown = _attackDuration;

                SetProgressBarForAttack();
            }
            else if (closestBuilding != null)
            {

                if (closestBuilding is Castle castle)
                {
                    castle.StealMagicalArtifact(this);
                }

                closestBuilding.Health -= _damageAgainstBuildings;
                Cooldown = _attackDuration;

                SetProgressBarForAttack();
            }
        }
    }

    private void AttackTarget(ITarget target, GameTime gameTime)
    {
        // TODO: this doesn't seem to work when diagonal
        Interact(gameTime);
    }

    private void RangedAttackTarget(ITarget target, GameTime gameTime)
    {
        // Vector2 toTarget = nextMove.Target.TDObject.Transform.LocalPosition.GetXY() - TDObject.Transform.LocalPosition.GetXY();
        // if (toTarget.LengthSquared() > _attackRangeSquared-1f)
        // {
        //     if (toTarget.LengthSquared() > 1f) toTarget.Normalize();
        //     Move(toTarget, gameTime);
        // }
        // else
        {
            if (Cooldown <= 0f)
            {
                ShootProjectile(target);

                Cooldown = _attackDuration;

                SetProgressBarForAttack();
            }
        }
    }

    protected virtual void ShootProjectile(ITarget target)
    {
        // nextMove.Target.Health -= _damageAgainstBuildings;
        TDObject arrowObject = PrefabFactory.CreatePrefab(PrefabType.Arrow);
        Projectile arrow = arrowObject.GetComponent<Projectile>();

        //TODO: if shot from enemy height, enemy hits itself ...
        arrow.StartPosition = TDObject.Transform.Position + 1.25f * Vector3.Backward;
        arrow.TargetCharacter = target;
        arrow.Damage = _damageAgainstBuildings;
        arrow.Speed = ProjectileSpeed;
        arrow.IsEvilArrow = true;
    }

    private void MoveToTarget(Vector2 localPosition, GameTime gameTime)
    {
        Vector2 direction = localPosition - TDObject.Transform.LocalPosition.GetXY();
        if (direction.LengthSquared() > 1f) direction.Normalize();
        Move(direction, gameTime);
    }

    private void SetProgressBarForAttack()
    {
        SetProgressBar(_attackDuration);
    }
}
