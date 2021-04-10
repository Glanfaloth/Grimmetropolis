using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class Enemy : Character
{
    private EnemyController _controller;
    private float _walkSpeed;
    private float _rotateSpeed;
    private float _baseHealth;
    private float _damageAgainstPlayers;
    private float _damageAgainstBuildings;
    private float _attackRange;
    private float _attackDuration;

    protected abstract EnemyMove.Type Actions { get; }

    public abstract String MeshName { get; }
    public virtual String TextureName => "ColorPaletteTexture";

    public override float WalkSpeed => _walkSpeed;

    protected override float RotateSpeed => _rotateSpeed;

    public override float BaseHealth => _baseHealth;

    public float AttackRange => _attackRange;
    public float AttackDuration => _attackDuration;

    public override Vector3 OffsetTarget { get; } = .5f * Vector3.Backward;

    public void SetBaseStats(Config.EnemyStats stats)
    {
        _walkSpeed = stats.WALK_SPEED;
        _rotateSpeed = stats.ROTATE_SPEED;
        _baseHealth = stats.HEALTH;
        _damageAgainstPlayers = stats.DAMAGE_AGAINST_PLAYER;
        _damageAgainstBuildings = stats.DAMAGE_AGAINST_BUILDINGS;
        _attackRange = stats.ATTACK_RANGE;
        _attackDuration = stats.ATTACK_DURATION;
    }

    public override void Initialize()
    {
        base.Initialize();

        _controller = GameManager.Instance.EnemyController;

        GameManager.Instance.Enemies.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        if (_controller == null) return;

        EnemyMove nextMove = _controller.ComputeNextMove(TDObject.Transform.LocalPosition.GetXY(), Actions, _attackRange);

        switch (nextMove.MovementType)
        {
            case EnemyMove.Type.None:
                Debug.WriteLine("ERROR: no valid move found for enemy");
                break;
            case EnemyMove.Type.StealArtifact:
                // TODO: implement win condition
                break;
            case EnemyMove.Type.Run:
                MoveToTarget((RunMove)nextMove, gameTime);
                break;
            case EnemyMove.Type.Attack:
                AttackTarget((AttackMove)nextMove, gameTime);
                break;
            case EnemyMove.Type.RangedAttack:
                RangedAttackTarget((RangedAttackMove)nextMove, gameTime);
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

        if (Cooldown <= 0f)
        {
            if (closestPlayer != null)
            {
                closestPlayer.Health -= _damageAgainstPlayers;
                Cooldown = _attackDuration;

                SetProgressBarForAttack();
            }
            else if (closestBuilding != null)
            {
                closestBuilding.Health -= _damageAgainstBuildings;
                Cooldown = _attackDuration;

                SetProgressBarForAttack();
            }
        }
    }

    private void AttackTarget(AttackMove nextMove, GameTime gameTime)
    {
        Interact(gameTime);
    }

    private void RangedAttackTarget(RangedAttackMove nextMove, GameTime gameTime)
    {
        // TODO: add projectile
        Interact(gameTime);
    }

    private void MoveToTarget(RunMove runMove, GameTime gameTime)
    {
        Vector2 direction = runMove.Destination.TDObject.Transform.LocalPosition.GetXY() - TDObject.Transform.LocalPosition.GetXY();
        if (direction.LengthSquared() > 1f) direction.Normalize();
        Move(direction, gameTime);
    }

    private void SetProgressBarForAttack()
    {
        IsShowingCooldown = true;

        ProgressBar.CurrentProgress = Cooldown;
        ProgressBar.MaxProgress = _attackDuration;
        ProgressBar.SetProgressBar();
        ProgressBar.Show();
    }
}
