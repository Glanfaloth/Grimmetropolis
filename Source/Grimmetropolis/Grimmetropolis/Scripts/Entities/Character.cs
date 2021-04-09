using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

public abstract class Character : TDComponent
{
    public abstract float WalkSpeed { get; }
    protected abstract float RotateSpeed { get; }
    public abstract float BaseHealth { get; }
    public abstract Vector3 OffsetTarget { get; }

    // TODO: those should be different for enemies
    public float LookingAngle = 0f;
    public float CurrentWalkSpeed = 0f;

    // health will be set during Initialize
    private float _health = -1;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    public Item[] Items = new Item[3];

    public TDCylinderCollider InteractionCollider;
    protected List<Tuple<TDCollider, float>> _colliderList = new List<Tuple<TDCollider, float>> ();

    // TODO: add shooting range collider

    public override void Initialize()
    {
        base.Initialize();

        Health = BaseHealth;

        InteractionCollider.collisionEvent += GetClosestCollider;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, LookingAngle);
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
            if (targetAngle - LookingAngle > MathHelper.Pi) LookingAngle += MathHelper.TwoPi;
            else if (LookingAngle - targetAngle > MathHelper.Pi) LookingAngle -= MathHelper.TwoPi;

            if (targetAngle > LookingAngle) LookingAngle = MathHelper.Min(LookingAngle + RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);
            else if (targetAngle < LookingAngle) LookingAngle = MathHelper.Max(LookingAngle - RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);

            TDObject.Transform.LocalPosition += WalkSpeed * new Vector3(direction, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, LookingAngle);
        }
    }

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null && mapTile.Item == null && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST)))
        {
            GameManager.Instance.ResourcePool -= new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST);
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, GameManager.Instance.StructureTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;
        }
    }

    protected void TakeDrop()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null)
        {
            if (Items[0] != null && mapTile.Item == null) Items[0].Drop();
            else if (Items[0] == null && mapTile.Item != null) mapTile.Item.TakeItem(this);
        }
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = InteractionCollider == collider2 ? collider1 : collider2;
        _colliderList.Add(new Tuple<TDCollider, float>(oppositeCollider, intersection));
    }
}
