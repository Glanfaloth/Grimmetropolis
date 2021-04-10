using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

public abstract class Character : TDComponent, ITDTarget
{
    public abstract float WalkSpeed { get; }
    protected abstract float RotateSpeed { get; }
    public abstract float BaseHealth { get; }
    public abstract Vector3 OffsetTarget { get; }

    // TODO: those should be different for enemies
    public float LookingAngle = 0f;
    public float CurrentWalkSpeed = 0f;

    public TDMesh Mesh;

    // health will be set during Initialize
    private float _health = -1;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_healthBar != null)
            {
                _healthBar.CurrentProgress = _health;
                _healthBar.Show();
            }
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    public Item[] Items = new Item[3];

    public TDCylinderCollider InteractionCollider;
    protected List<Tuple<TDCollider, float>> _colliderList = new List<Tuple<TDCollider, float>> ();

    // TODO: add shooting range collider

    private HealthBar _healthBar = null;
    protected ProgressBar ProgressBar = null;
    protected bool IsShowingCooldown = false;

    private float _cooldown = 0f;
    public float Cooldown
    {
        get => _cooldown;
        set
        {
            _cooldown = value;
            if (ProgressBar != null && IsShowingCooldown)
            {
                ProgressBar.CurrentProgress = _cooldown;
                ProgressBar.SetProgressBar();
            }
        }
    }

    private float _progress = 0f;
    public float Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            if (ProgressBar != null && !IsShowingCooldown)
            {
                ProgressBar.CurrentProgress = _progress;
                ProgressBar.SetProgressBar();
            }
        }
    }

    TDObject ITDTarget.TDObject => TDObject;

    public override void Initialize()
    {
        base.Initialize();

        Health = BaseHealth;

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 2.5f * Vector3.Backward;
        _healthBar = healthBarObject.GetComponent<HealthBar>();
        _healthBar.CurrentProgress = Health;
        _healthBar.MaxProgress = BaseHealth;

        TDObject progessBarObject = PrefabFactory.CreatePrefab(PrefabType.ProgressBar, TDObject.Transform);
        progessBarObject.RectTransform.Offset = 2f * Vector3.Backward;
        ProgressBar = progessBarObject.GetComponent<ProgressBar>();
        ProgressBar.CurrentProgress = Progress;
        ProgressBar.MaxProgress = 1f;

        InteractionCollider.collisionEvent += GetClosestCollider;

        TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, LookingAngle);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Cooldown <= 0f)
        {
            if (IsShowingCooldown)
            {
                IsShowingCooldown = false;
                ProgressBar.Hide();
            }
        }

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

    protected virtual void Interact(GameTime gameTime) { }

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

    public void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
    }
}
