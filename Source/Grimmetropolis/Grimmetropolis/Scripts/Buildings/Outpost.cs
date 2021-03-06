using Microsoft.Xna.Framework;

using System.Diagnostics;

public class Outpost : Building
{
    public override ResourcePile GetResourceCost() => new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST);
    public override ResourcePile GetResourceUpkeep() => new ResourcePile(0, 0, Config.OUTPOST_FOOD_UPKEEP);
    public override float BuildTime => Config.OUTPOST_BUILD_VALUE;

    public TDCylinderCollider ShootingRange = null;

    private float _intersectionEnemy = 0f;
    private Enemy _closestEnemy = null;

    private float _cooldown = 0f;
    private float _interval = Config.OUTPOST_SHOOTING_RATE;
    private float _arrowSpeed = Config.OUTPOST_ARROW_SPEED;
    private float _arrowDamage = Config.OUTPOST_ARROW_DAMAGE;


    public override bool MissingUpkeep
    {
        get => base.MissingUpkeep;
        set
        {
            base.MissingUpkeep = value;
            _interval = (MissingUpkeep ? 2f : 1f) * Config.OUTPOST_SHOOTING_RATE;
            ShootingRange.Radius = (MissingUpkeep ? .5f : 1f) * Config.OUTPOST_SHOOTING_RANGE;
        }
    }

    public override void Initialize()
    {
        BaseHealth = Config.OUTPOST_HEALTH;
        Health = Config.OUTPOST_HEALTH;

        ShootingRange.IsTrigger = true;
        ShootingRange.Radius = (MissingUpkeep ? .5f : 1f) * Config.OUTPOST_SHOOTING_RANGE;
        ShootingRange.Height = 2f;
        ShootingRange.Offset = Vector3.Zero;
        ShootingRange.collisionEvent += GetClosestCollider;

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsPreview || IsBlueprint) return;

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
        ShootingRange = null;

        base.Destroy();
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = ShootingRange == collider2 ? collider1 : collider2;
        Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
        if (enemy != null && !(enemy is TutorialAdvisor))
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
