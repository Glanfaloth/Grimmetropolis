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

    public bool ActiveInput = true;

    public ResourceDeposit LastClosestResourceDeposit = null;
    public bool NeedsToShowHarvestProgress = false;

    private Enemy _closestEnemy = null;
    private MapTile _collidingMapTile = null;

    private BuildMenu _buildMenu = null;


    public override void Initialize()
    {
        base.Initialize();

        HealthBar.TDObject.Destroy();
        UIManager.Instance.AddPlayerDisplay(this);

        TDObject buildMenuObject = PrefabFactory.CreatePrefab(PrefabType.BuildMenu, TDObject.Transform);
        _buildMenu = buildMenuObject.GetComponent<BuildMenu>();
        _buildMenu.Player = this;
        buildMenuObject.RectTransform.Offset = 2f * Vector3.Backward;

        GameManager.Instance.Players.Add(this);
    }

    public override void Update(GameTime gameTime)
    {
        HighlightClosestCharacter();
        HighlightMapTile();

        Vector2 inputDirection = Input.MoveDirection();
        Move(new Vector2(-inputDirection.Y, inputDirection.X), gameTime);

        if (ActiveInput)
        {

            if (Input.ActionPressed()) Interact(gameTime);
            else ResetProgressBarForProgress();
            if (Input.CancelPressed()) Drop();
            if (Input.BuildModePressed() && Cooldown <= 0f)
                _buildMenu.Show();
        }

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

        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        Item _closestItem = mapTile.Item;
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
            if (Items[0] != null) Items[0].InteractWithCharacter(gameTime, _closestEnemy);
            else
            {
                _closestEnemy.Health -= .25f * Config.PLAYER_DAMAGE;
                Cooldown = 1.5f * Config.PLAYER_ATTACK_DURATION;

                ResetProgressBarForProgress();
                SetProgressForCooldown();
            }

        }
        else if (_closestItem != null)
        {
            Take();
        }
        else if (_closestStructure != null && Cooldown <= 0f)
        {
            if (Items[0] != null) Items[0].InteractWithStructure(gameTime, _closestStructure);
            else
            {
                Build(gameTime);
            }
        }
    }

    public void BuildBlueprint(Building building)
    {
        if (Cooldown > 0)
        {
            return;
        }

        MapTile mapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        if (mapTile.Type == MapTileType.Ground
            && mapTile.Structure == null
            && mapTile.Item == null
            && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, building.GetResourceCost()))
        {
            GameManager.Instance.ResourcePool -= building.GetResourceCost();
            building.Position = mapTile.Position;
            building.SetAsBlueprint();

            Cooldown = Config.PLAYER_PLACE_BUILDING_COOLDOWN;
            ResetProgressBarForProgress();
            SetProgressBar(Cooldown);
        }
    }

    public void Build(GameTime gameTime)
    {
        if (Cooldown > 0)
        {
            return;
        }

        if (_collidingMapTile.Type == MapTileType.Ground && _collidingMapTile.Structure is Building building)
        {
            if(building.TryBuild(Config.PLAYER_BUILD_STRENGTH) && building.Mesh.IsBlueprint)
            {
                Cooldown = Config.PLAYER_BUILD_COOLDONW;
                ResetProgressBarForProgress();
                SetProgressBar(Cooldown);
            }
            else if (building.TryRepair(Config.PLAYER_BUILD_STRENGTH))
            {
                Cooldown = 2f * Config.PLAYER_BUILD_COOLDONW;
                ResetProgressBarForProgress();
                SetProgressBar(Cooldown);
            }
        }
    }

    public void ResetProgressBarForProgress()
    {
        LastClosestResourceDeposit = null;
        if (!IsShowingCooldown) ProgressBar.Hide();
    }
    public void SetProgressForCooldown()
    {
        SetProgressBar(Cooldown);
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
