using Microsoft.Xna.Framework;

public class Outpost : Building
{
    public static new ResourcePile ResourceCost = new ResourcePile(1f, 1f);

    public TDCylinderCollider ShootingRange = null;

    private float _intersectionEnemy = 0f;
    private Enemy _closestEnemy = null;

    private float _cooldown = 0f;
    private float _interval = .5f;

    public override void Initialize()
    {
        ResourceCost = new ResourcePile(1f, 1f);

        ShootingRange.collisionEvent += GetClosestCollider;

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
}
