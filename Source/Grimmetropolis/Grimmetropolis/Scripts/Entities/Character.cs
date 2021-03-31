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

    private float _intersectionEnemy = 0f;
    private float _intersectionBuilding = 0f;
    private float _intersectionResource = 0f;
    private Enemy _closestEnemy = null;
    private Building _closestBuilding = null;
    private ResourceDeposit _closestResource = null;

    public override void Initialize()
    {
        base.Initialize();

        InteractionCollider.collisionCylinderCylinderEvent += GetClosestCylinder;
        InteractionCollider.collisionCylinderCuboidEvent += GetClosestCuboid;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _intersectionEnemy = 0f;
        _intersectionBuilding = 0f;
        _intersectionResource = 0f;
        _closestEnemy = null;
        _closestBuilding = null;
        _closestResource = null;
    }

    public override void Destroy()
    {
        InteractionCollider.collisionCylinderCylinderEvent -= GetClosestCylinder;
        InteractionCollider.collisionCylinderCuboidEvent -= GetClosestCuboid;

        base.Destroy();
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

    protected void Interact()
    {
        if (_closestEnemy != null)
        {
            _closestEnemy.Health -= 1f;
        }
        else if (_closestBuilding != null)
        {
            _closestBuilding.Health -= 1f;
        }
        else if (_closestResource != null)
        {
            _closestResource.GetResources();
        }
    }

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null)
        {
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.Outpost, GameManager.Instance.StructureTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;
        }
    }

    // TODO: adapt this for player and enemy
    private void GetClosestCylinder(TDCylinderCollider cylinder1, TDCylinderCollider cylinder2, float intersection)
    {
        TDCylinderCollider oppositeCollider = InteractionCollider == cylinder2 ? cylinder1 : cylinder2;
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

    private void GetClosestCuboid(TDCylinderCollider cylinder, TDCuboidCollider cuboid, Vector2 closest, float intersection)
    {
        Building building = cuboid.TDObject.GetComponent<Building>();
        if (building != null)
        {
            if (_intersectionBuilding < intersection)
            {
                _intersectionBuilding = intersection;
                _closestBuilding = building;
            }
        }
        ResourceDeposit resource = cuboid.TDObject.GetComponent<ResourceDeposit>();
        if (resource != null)
        {
            if (_intersectionResource < intersection)
            {
                _intersectionResource = intersection;
                _closestResource = resource;
            }
        }
    }
}
