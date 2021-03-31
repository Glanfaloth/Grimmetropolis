using Microsoft.Xna.Framework;

using System;

public abstract class Character : TDComponent
{
    private float _lookingAngle = 0f;
    private float _walkSpeed = 4f;
    private float _rotateSpeed = 3f * MathHelper.Pi;

    private float _health = 3f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    public TDCylinderCollider InteractionCollider;

    private float _intersectionEnemy = 0f;
    private Enemy _closestEnemy = null;
    private float _intersectionStructure = 0f;
    private Structure _closestStructure = null;

    public override void Initialize()
    {
        base.Initialize();

        InteractionCollider.collisionEvent += GetClosestCollider;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _intersectionEnemy = 0f;
        _intersectionStructure = 0f;
        _closestEnemy = null;
        _closestStructure = null;
    }

    public override void Destroy()
    {
        InteractionCollider.collisionEvent -= GetClosestCollider;

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
        else if (_closestStructure != null)
        {
            switch (_closestStructure)
            {
                case Building building:
                    building.Health -= 1f;
                    break;
                case ResourceDeposit resourceDeposit:
                    resourceDeposit.GetResources();
                    break;
            }
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

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = InteractionCollider == collider2 ? collider1 : collider2;
        Enemy enemy = oppositeCollider.TDObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (_intersectionEnemy < intersection)
            {
                _intersectionEnemy = intersection;
                _closestEnemy = enemy;
            }
        }
        else
        {
            Structure structure = oppositeCollider.TDObject.GetComponent<Structure>();
            if (structure != null)
            {
                if (_intersectionStructure < intersection)
                {
                    _intersectionStructure = intersection;
                    _closestStructure = structure;
                }
            }
        }
    }
}
