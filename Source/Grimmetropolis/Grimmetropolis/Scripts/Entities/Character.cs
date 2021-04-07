using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

public abstract class Character : TDComponent
{
    // TODO: those should be different for enemies
    private float _lookingAngle = 0f;
    private float _walkSpeed = Config.PLAYER_WALK_SPEED;
    private float _rotateSpeed = Config.PLAYER_ROTATE_SPEED;

    private float _health = Config.PLAYER_HEALTH;
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
    protected List<Tuple<TDCollider, float>> _colliderList = new List<Tuple<TDCollider, float>> ();

    public override void Initialize()
    {
        base.Initialize();

        InteractionCollider.collisionEvent += GetClosestCollider;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, _lookingAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _colliderList.Clear();
    }

    public override void Destroy()
    {
        InteractionCollider.collisionEvent -= GetClosestCollider;

        base.Destroy();
    }

    protected void Move(Vector2 direction, GameTime gameTime)
    {
        // TODO: EPSILON should probably be larger
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

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, Outpost.ResourceCost))
        {
            GameManager.Instance.ResourcePool -= Outpost.ResourceCost;
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.Outpost, GameManager.Instance.StructureTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;
        }
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = InteractionCollider == collider2 ? collider1 : collider2;
        _colliderList.Add(new Tuple<TDCollider, float>(oppositeCollider, intersection));
    }
}
