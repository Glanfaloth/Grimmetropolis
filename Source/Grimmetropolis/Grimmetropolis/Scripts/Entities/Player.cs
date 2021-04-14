using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

public class Player : Character
{
    public TDInput Input;

    public override float WalkSpeed => Config.PLAYER_WALK_SPEED;

    protected override float RotateSpeed => Config.PLAYER_ROTATE_SPEED;

    public override float BaseHealth => Config.PLAYER_HEALTH;

    public override Vector3 OffsetTarget => .5f * Vector3.Backward;

    private ResourceDeposit _lastClosestResourceDeposit = null;
    private bool _needsToShowHarvestProgress = false;

    private Enemy _closestEnemy = null;
    private MapTile _collidingMapTile = null;

    public override void Initialize()
    {
        base.Initialize();

        HealthBar.TDObject.Destroy();
        UIManager.Instance.AddPlayerDisplay(this);

        GameManager.Instance.Players.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 inputDirection = Input.GetMoveDirection();
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);

        HighlightClosestCharacter();
        HighlightMapTile();

        if (Input.IsSpecialAbilityPressed()) Interact(gameTime);
        else ResetProgressBarForProgress();
        if (Input.IsUseItemPressed()) Build();
        if (Input.IsSwapItemPressed()) TakeDrop();

        base.Update(gameTime);
    }

    public override void Destroy()
    {
        base.Destroy();

        Input = null;
        GameManager.Instance.Players.Remove(this);
    }


    protected override void Interact(GameTime gameTime)
    {
        base.Interact(gameTime);

        float closestStructureDistance = float.MaxValue;
        Structure _closestStructure = null;
        foreach (Tuple<TDCollider, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCuboidCollider && closestStructureDistance > colliderEntry.Item2)
            {
                Structure structure = colliderEntry.Item1.TDObject?.GetComponent<MapTile>().Structure;
                if (structure != null)
                {
                    closestStructureDistance = colliderEntry.Item2;
                    _closestStructure = structure;
                }
            }
        }
        
        if (_closestEnemy != null && Cooldown <= 0f)
        {
            _closestEnemy.Health -= Config.PLAYER_DAMAGE;
            Cooldown = Config.PLAYER_ATTACK_DURATION;

            ResetProgressBarForProgress();
            SetProgressBarForAttack();

        }
        else if (_closestStructure != null)
        {
            if (Cooldown <= 0f && _closestStructure is Building closestBuilding)
            {
                closestBuilding.Health -= Config.PLAYER_DAMAGE;
                Cooldown = Config.PLAYER_ATTACK_DURATION;

                ResetProgressBarForProgress();
                SetProgressBarForAttack();
            }
            else if (_closestStructure is ResourceDeposit closestResourceDeposit)
            {
                if (closestResourceDeposit != _lastClosestResourceDeposit)
                {
                    _needsToShowHarvestProgress = true;
                    _lastClosestResourceDeposit = closestResourceDeposit;
                    Progress = 0f;
                }

                if (!IsShowingCooldown && _needsToShowHarvestProgress)
                {
                    _needsToShowHarvestProgress = false;

                    ProgressBar.MaxProgress = _lastClosestResourceDeposit.HarvestTime;
                    ProgressBar.Show();
                }

                Progress += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Progress >= _lastClosestResourceDeposit.HarvestTime)
                {
                    _lastClosestResourceDeposit.HarvestResource();
                    Progress -= _lastClosestResourceDeposit.HarvestTime;
                }
            }
        }
        else ResetProgressBarForProgress();
    }

    protected void Build()
    {
        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (Cooldown <= 0
            && mapTile.Type == MapTileType.Ground
            && mapTile.Structure == null
            && mapTile.Item == null
            && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST)))
        {
            GameManager.Instance.ResourcePool -= new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST);
            TDObject buildingObject = PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost, GameManager.Instance.StructureTransform);
            Building building = buildingObject.GetComponent<Building>();
            building.Position = mapTile.Position;

            Cooldown = Config.PLAYER_PLACE_BUILDING_COOLDOWN;
            ResetProgressBarForProgress();
            SetProgressBarForBuild();
        }
    }

    private void ResetProgressBarForProgress()
    {
        _lastClosestResourceDeposit = null;
        if (!IsShowingCooldown) ProgressBar.Hide();
    }
    private void SetProgressBarForAttack()
    {
        SetProgressBar(Config.PLAYER_ATTACK_DURATION);
    }

    private void SetProgressBarForBuild()
    {
        SetProgressBar(Config.PLAYER_PLACE_BUILDING_COOLDOWN);
    }

    private void HighlightClosestCharacter()
    {
        _closestEnemy?.Highlight(false);

        float closestEnemyDistance = float.MaxValue;
        _closestEnemy = null;

        foreach (Tuple<TDCollider, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCylinderCollider && closestEnemyDistance > colliderEntry.Item2)
            {
                Enemy enemy = colliderEntry.Item1.TDObject?.GetComponent<Enemy>();
                if (enemy != null)
                {
                    closestEnemyDistance = colliderEntry.Item2;
                    _closestEnemy = enemy;
                }
            }
        }
        _closestEnemy?.Highlight(true);
    }

    private void HighlightMapTile()
    {
        _collidingMapTile?.Highlight(false);
        _collidingMapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        _collidingMapTile.Highlight(true);
    }
}
