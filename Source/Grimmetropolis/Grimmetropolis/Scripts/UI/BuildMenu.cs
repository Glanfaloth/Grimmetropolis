using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

public enum SelectableBuilding
{
    Outpost,
    Wall,
    Farm,
    Bridge,
    Hospital,
    ResourceBuilding
}

public class BuildMenu : TDComponent
{
    public Player Player;

    public TDSprite BuildSprite;
    public TDSprite Icon;

    public bool IsShowing = true;

    private SelectableBuilding _currentBuilding;
    private bool _activeControl = true;

    private float _cooldown = 0f;
    private float _cooldownDuration = .2f;

    private Building _previewBuilding = null;

    public override void Initialize()
    {
        base.Initialize();

        Icon.Texture = GetIcon(_currentBuilding);

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_activeControl && _cooldown <= 0f)
        {
            if (Player.Input.CycleRightPressed()) ShowNextBuilding();
            if (Player.Input.CycleLeftPressed()) ShowPreviousBuilding();

            if (Player.Input.ActionPressed())
            {
                Building building = GetBuilding(_currentBuilding);
                building.Position = _previewBuilding.Position;
                Player.BuildBlueprint(building);
                Hide();
            }

            if (IsShowing && _previewBuilding != null)
            {
                _previewBuilding.Position = GameManager.Instance.Map.GetMapTile(Player.InteractionCollider.CenterXY).Position;
                _previewBuilding.SetMapTransform();
            }

            if (Player.Input.BuildModePressed()) Hide();
        }
    }

    public void Show()
    {
        if (IsShowing) return;
        IsShowing = true;

        BuildSprite.IsShowing = IsShowing;
        Icon.IsShowing = IsShowing;

        _previewBuilding = GetBuilding(_currentBuilding);
        _previewBuilding.Position = GameManager.Instance.Map.GetMapTile(Player.InteractionCollider.CenterXY).Position;
        _previewBuilding.IsPreview = true;

        OvertakeControl();
    }

    public void Hide()
    {
        if (!IsShowing) return;
        IsShowing = false;

        BuildSprite.IsShowing = IsShowing;
        Icon.IsShowing = IsShowing;

        _previewBuilding?.TDObject.Destroy();
        ReturnControl();
    }

    private void OvertakeControl()
    {
        if (_activeControl) return;
        _activeControl = true;

        Player.ActiveInput = false;
        _cooldown = _cooldownDuration;
    }

    private void ReturnControl()
    {
        Player.ActiveInput = true;
        _activeControl = false;

        Player.Cooldown = _cooldownDuration;
        Player.SetProgressForCooldown();
    }

    private void ShowNextBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)_currentBuilding == selectableBuildingCount - 1) _currentBuilding = 0;
        else _currentBuilding++;

        Icon.Texture = GetIcon(_currentBuilding);

        _previewBuilding.TDObject.Destroy();
        _previewBuilding = GetBuilding(_currentBuilding);
        _previewBuilding.IsPreview = true;

        _cooldown = _cooldownDuration;
    }

    private void ShowPreviousBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)_currentBuilding == 0) _currentBuilding = (SelectableBuilding)selectableBuildingCount - 1;
        else _currentBuilding--;

        Icon.Texture = GetIcon(_currentBuilding);

        _previewBuilding.TDObject.Destroy();
        _previewBuilding = GetBuilding(_currentBuilding);
        _previewBuilding.IsPreview = true;

        _cooldown = _cooldownDuration;
    }

    private Texture2D GetIcon(SelectableBuilding buildingIcon)
    {
        if (Icon == null) return null;

        return buildingIcon switch
        {
            SelectableBuilding.Outpost => TDContentManager.LoadTexture("UIBuildingOutpostIcon"),
            SelectableBuilding.Wall => TDContentManager.LoadTexture("UIBuildingWallIcon"),
            SelectableBuilding.Farm => TDContentManager.LoadTexture("UIBuildingFarmIcon"),
            SelectableBuilding.Bridge => TDContentManager.LoadTexture("UIBuildingBridgeIcon"),
            SelectableBuilding.Hospital => TDContentManager.LoadTexture("UIBuildingHospitalIcon"),
            SelectableBuilding.ResourceBuilding => TDContentManager.LoadTexture("UIBuildingResourceBuildingIcon"),
            _ => TDContentManager.LoadTexture("UIBuildingOutpostIcon")
        };
    }

    private Building GetBuilding(SelectableBuilding buildingIcon)
    {
        if (Icon == null) return null;

        Building building = buildingIcon switch
        {
            SelectableBuilding.Outpost => PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost).GetComponent<Outpost>(),
            SelectableBuilding.Wall => PrefabFactory.CreatePrefab(PrefabType.BuildingWall).GetComponent<Wall>(),
            SelectableBuilding.Farm => PrefabFactory.CreatePrefab(PrefabType.BuildingFarm).GetComponent<Farm>(),
            SelectableBuilding.Bridge => PrefabFactory.CreatePrefab(PrefabType.BuildingBridge).GetComponent<Bridge>(),
            SelectableBuilding.Hospital => PrefabFactory.CreatePrefab(PrefabType.BuildingHospital).GetComponent<Hospital>(),
            SelectableBuilding.ResourceBuilding => PrefabFactory.CreatePrefab(PrefabType.BuildingResourceBuilding).GetComponent<ResourceBuilding>(),
            _ => PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost).GetComponent<Outpost>()
        };
        return building;
    }
}
