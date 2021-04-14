﻿using Microsoft.Xna.Framework;

public class Outpost : Building
{
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST);

    public TDCylinderCollider ShootingRange = null;

    private float _intersectionEnemy = 0f;
    private Enemy _closestEnemy = null;

    private float _cooldown = 0f;
    private float _interval = Config.OUTPOST_SHOOTING_RATE;
    private float _arrowSpeed = Config.OUTPOST_ARROW_SPEED;
    private float _arrowDamage = Config.OUTPOST_ARROW_DAMAGE;

    public override float BuildTime => Config.OUTPOST_BUILD_VALUE;

    public override void Initialize()
    {
        BaseHealth = Config.OUTPOST_HEALTH;
        Health = Config.OUTPOST_HEALTH;

        ShootingRange.IsTrigger = true;
        ShootingRange.Radius = Config.OUTPOST_SHOOTING_RANGE;
        ShootingRange.Height = 1f;
        ShootingRange.Offset = Vector3.Zero;
        ShootingRange.collisionEvent += GetClosestCollider;

        base.Initialize();
    }

    protected override void DoUpdate(GameTime gameTime)
    {
        _cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_closestEnemy != null && _cooldown <= 0f)
        {
            ShootArrow();
            _cooldown = _interval;
        }

        _intersectionEnemy = 0f;
        _closestEnemy = null;
    }

    public override void Destroy()
    {
        ShootingRange.collisionEvent -= GetClosestCollider;

        base.Destroy();
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = ShootingRange == collider2 ? collider1 : collider2;
        Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (_intersectionEnemy < intersection)
            {
                _intersectionEnemy = intersection;
                _closestEnemy = enemy;
            }
        }
    }

    private void ShootArrow()
    {
        TDObject arrowObject = PrefabFactory.CreatePrefab(PrefabType.Arrow);
        Projectile arrow = arrowObject.GetComponent<Projectile>();
        arrow.StartPosition = TDObject.Transform.Position + 2.25f * Vector3.Backward;
        arrow.TargetCharacter = _closestEnemy;
        arrow.Damage = _arrowDamage;
        arrow.Speed = _arrowSpeed;
    }
}
