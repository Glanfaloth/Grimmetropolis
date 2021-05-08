using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

public abstract class Character : TDComponent, ITarget
{
    public abstract float WalkSpeed { get; }
    protected abstract float RotateSpeed { get; }
    public abstract float BaseHealth { get; }
    public abstract Vector3 OffsetTarget { get; }

    // TODO: those should be different for enemies
    public float LookingAngle = 0f;
    public float CurrentWalkSpeed = 0f;

    public EntityAnimation Animation;
    public TDMesh Mesh;

    // health will be set during Initialize
    // ToDo: -1 introduces bugs for EnemyGroup
    private float _health = int.MaxValue;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (HealthBar != null)
            {
                HealthBar.CurrentProgress = _health;
                HealthBar.Show();
            }
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    public Item[] Items = new Item[3];

    public TDCylinderCollider InteractionCollider;
    protected List<Tuple<TDCollider, float>> _colliderList = new List<Tuple<TDCollider, float>> ();

    // TODO: add shooting range collider

    public HealthBar HealthBar = null;
    public ProgressBar ProgressBar = null;
    public bool IsShowingCooldown = false;

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
                ProgressBar.Show();
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
                ProgressBar.Show();
            }
        }
    }

    TDObject ITarget.TDObject => TDObject;

    public override void Initialize()
    {
        base.Initialize();

        Health = BaseHealth;

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 2.5f * Vector3.Backward;
        HealthBar = healthBarObject.GetComponent<HealthBar>();
        HealthBar.CurrentProgress = Health;
        HealthBar.MaxProgress = BaseHealth;

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
        foreach (Item item in Items)
        {
            item?.Drop();
        }

        InteractionCollider.collisionEvent -= GetClosestCollider;

        base.Destroy();
    }

    protected void Move(Vector2 direction, GameTime gameTime)
    {
        CurrentWalkSpeed = direction.Length();
        if (CurrentWalkSpeed > 1e-5f)
        {
            CurrentWalkSpeed *= WalkSpeed;
            float targetAngle = MathF.Atan2(direction.Y, direction.X);
            if (targetAngle - LookingAngle > MathHelper.Pi) LookingAngle += MathHelper.TwoPi;
            else if (LookingAngle - targetAngle > MathHelper.Pi) LookingAngle -= MathHelper.TwoPi;

            if (targetAngle > LookingAngle) LookingAngle = MathHelper.Min(LookingAngle + RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);
            else if (targetAngle < LookingAngle) LookingAngle = MathHelper.Max(LookingAngle - RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, targetAngle);

            TDObject.Transform.LocalPosition += WalkSpeed * new Vector3(direction, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            TDObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, LookingAngle);
        }
    }

    protected virtual void Interact(GameTime gameTime) { }

    protected virtual void Take()
    {
        if (Cooldown > 0f) return;

        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null)
        {
            if (Items[0] == null && mapTile.Item != null) mapTile.Item.TakeItem(this);
        }
    }

    protected virtual void Drop()
    {
        if (Cooldown > 0f) return;

        MapTile mapTile = GameManager.Instance.Map.GetMapTile(TDObject.Transform.Position.GetXY());
        if (mapTile.Type == MapTileType.Ground && mapTile.Structure == null)
        {
            if (Items[0] != null && mapTile.Item == null) Items[0].Drop();
        }
    }

    private void GetClosestCollider(TDCollider collider1, TDCollider collider2, float intersection)
    {
        TDCollider oppositeCollider = InteractionCollider == collider2 ? collider1 : collider2;
        _colliderList.Add(new Tuple<TDCollider, float>(oppositeCollider, intersection));
    }

    public void Highlight(bool highlight)
    {
        if (Mesh != null) Mesh.Highlight(highlight);
        else Animation.Highlight(highlight);
        if (highlight) HealthBar.QuickShow();
        else HealthBar.QuickHide();
    }

    protected void SetProgressBar(float maxProgress)
    {
        IsShowingCooldown = true;

        ProgressBar.CurrentProgress = Cooldown;
        ProgressBar.MaxProgress = maxProgress;
        ProgressBar.Show();
    }
}
