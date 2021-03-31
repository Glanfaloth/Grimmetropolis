using Microsoft.Xna.Framework;

public class Outpost : Building
{
    public TDCylinderCollider ShootingRange = null;

    private float _intersectionEnemy = 0f;
    private Enemy _closestEnemy = null;

    private float _cooldown = 0f;
    private float _interval = .5f;

    public override void Initialize()
    {
        ResourceCost = new ResourcePile(1f, 1f);

        ShootingRange.collisionCylinderCylinderEvent += GetClosestCylinder;

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_closestEnemy != null && _cooldown <= 0f)
        {
            _closestEnemy.Health -= 1f;
            _cooldown = _interval;
        }

        _intersectionEnemy = 0f;
        _closestEnemy = null;
    }

    public override void Destroy()
    {
        ShootingRange.collisionCylinderCylinderEvent -= GetClosestCylinder;

        base.Destroy();
    }

    private void GetClosestCylinder(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2, float intersection)
    {
        TDCylinderCollider oppositeCollider = ShootingRange == cylinder2 ? cylinder1 : cylinder2;
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
}
