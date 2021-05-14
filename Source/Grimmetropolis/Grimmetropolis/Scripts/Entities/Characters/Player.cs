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

    // public TDSprite PlayerIcon = null;

    private Enemy _closestEnemy = null;
    private MapTile _interactionCollidingMapTile = null;
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
            if (Input.BuildModePressed() && Items[0] is ToolHammer && Cooldown <= 0f)
                _buildMenu.Show();
        }
        //UIManager.Instance.AddPlayerDisplay(this);
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
        Structure closestStructure = null;

        foreach (Tuple<TDCollider, float, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCuboidCollider && closestStructureDistance > colliderEntry.Item3)
            {
                Structure structure = colliderEntry.Item1.TDObject?.GetComponent<MapTile>().Structure;
                if (structure != null)
                {
                    closestStructureDistance = colliderEntry.Item3;
                    closestStructure = structure;
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
        else if (_collidingMapTile.Item != null)
        {
            Take();
        }
        else if (closestStructure != null && Cooldown <= 0f)
        {
            if (Items[0] != null) Items[0].InteractWithStructure(gameTime, closestStructure);
        }
    }

    public void BuildBlueprint(Building building)
    {
        if (Cooldown > 0)
        {
            return;
        }

        if (building.CheckPlacability(building.Position) && ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, building.GetResourceCost()))
        {
            GameManager.Instance.ResourcePool -= building.GetResourceCost();
            building.IsBlueprint = true;

            Cooldown = Config.PLAYER_PLACE_BUILDING_COOLDOWN;
            ResetProgressBarForProgress();
            SetProgressBar(Cooldown);
        }
        else building.TDObject.Destroy();
    }

    public void Build(GameTime gameTime, Building building)
    {
        if (Cooldown > 0)
        {
            return;
        }

        if(building.TryBuild(Config.PLAYER_BUILD_STRENGTH) && building.Mesh.IsPreview)
        {
            Cooldown = Config.PLAYER_BUILD_COOLDOWN;
            ResetProgressBarForProgress();
            SetProgressBar(Cooldown);
        }
        else if (building.TryRepair(Config.PLAYER_BUILD_STRENGTH))
        {
            Cooldown = 1.5f * Config.PLAYER_BUILD_COOLDOWN;
            ResetProgressBarForProgress();
            SetProgressBar(Cooldown);
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

        foreach (Tuple<TDCollider, float, float> colliderEntry in _colliderList)
        {
            if (colliderEntry.Item1 is TDCylinderCollider && closestEnemyDistance > colliderEntry.Item3)
            {
                Enemy enemy = colliderEntry.Item1.TDObject?.GetComponent<Enemy>();
                if (enemy != null)
                {
                    closestEnemyDistance = colliderEntry.Item3;
                    _closestEnemy = enemy;
                }
            }
        }
        _closestEnemy?.Highlight(true);
    }

    private void HighlightMapTile()
    {
        _interactionCollidingMapTile?.Highlight(false);
        // _interactionCollidingMapTile?.Structure?.Highlight(true);
        _collidingMapTile?.Item?.Highlight(false);
        _interactionCollidingMapTile = GameManager.Instance.Map.GetMapTile(InteractionCollider.CenterXY);
        _collidingMapTile = GameManager.Instance.Map.GetMapTile(Collider.CenterXY);
        if (_buildMenu.IsShowing)
        {
            _interactionCollidingMapTile.Highlight(true);
        }
        /*else if (_interactionCollidingMapTile?.Structure != null && !(_interactionCollidingMapTile?.Structure is Bridge))
        {
            _interactionCollidingMapTile.Structure.Highlight(true);
        }*/
        else if (_collidingMapTile.Item != null && Items[0] == null)
        {
            _collidingMapTile.Item.Highlight(true);
        }
    }
}
