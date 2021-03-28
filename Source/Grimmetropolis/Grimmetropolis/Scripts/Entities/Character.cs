using Microsoft.Xna.Framework;

using System;

public abstract class Character : TDComponent
{
    public float _health = 3f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    private float _lookingAngle = 0f;

    private float _walkSpeed = 4f;
    private float _rotateSpeed = 3f * MathHelper.Pi;

    public TDCylinderCollider InteractionCollider;

    private float largestIntersection = 0f;
    private Enemy closestEnemy = null;

    public override void Initialize()
    {
        base.Initialize();

        InteractionCollider.collisionCylinderCylinderEvent += GetClosestEnemy;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        largestIntersection = 0f;
        closestEnemy = null;
    }

    protected void Move(Vector2 direction, GameTime gameTime)
    {
        if (direction.LengthSquared() > float.Epsilon)
        {
            float targetAngle = MathF.Atan2(direction.Y, direction.X);
            if (targetAngle - _lookingAngle > MathHelper.Pi) _lookingAngle += MathHelper.TwoPi;
            else if (_lookingAngle - targetAngle > MathHelper.Pi) _lookingAngle -= MathHelper.TwoPi;

            if (targetAngle > _lookingAngle) _lookingAngle = MathHelper.Min(_lookingAngle + _rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);
            else if (targetAngle < _lookingAngle) _lookingAngle = MathHelper.Max(_lookingAngle - _rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);

            TDObject.Transform.LocalPosition += _walkSpeed * new Vector3(direction, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
        }
    }

    protected void Attack()
    {
        if (closestEnemy != null)
        {
            closestEnemy.Health -= 1f;
        }
    }

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Building == null)
        {
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.Outpost, GameManager.Instance.BuildingTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;
        }
    }

    // TODO: adapt this for player and enemy
    private void GetClosestEnemy(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2, float intersection)
    {
        TDCylinderCollider oppositeCollider = InteractionCollider == cylinder2 ? cylinder1 : cylinder2;
        Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (largestIntersection < intersection)
            {
                largestIntersection = intersection;
                closestEnemy = enemy;
            }            
        }
    }
}
