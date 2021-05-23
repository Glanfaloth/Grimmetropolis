using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

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

    public TDText Title;

    public TDText WoodCost;
    public TDText StoneCost;

    public bool IsShowing = true;

    public SelectableBuilding CurrentBuilding;
    private bool _activeControl = true;

    private float _cooldown = 0f;
    private float _cooldownDuration = .2f;

    private Building _previewBuilding = null;

    private readonly Dictionary<SelectableBuilding, ResourcePile> _costs = new Dictionary<SelectableBuilding, ResourcePile>();
    public readonly List<TDUI> UiElements = new List<TDUI>();

    public override void Initialize()
    {
        base.Initialize();

        InitializeCosts();
        SetDescription(CurrentBuilding);

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
                Building building = GetBuilding(CurrentBuilding);
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

        foreach (var item in UiElements)
        {
            item.IsShowing = IsShowing;
        }

        _previewBuilding = GetBuilding(CurrentBuilding);
        _previewBuilding.Position = GameManager.Instance.Map.GetMapTile(Player.InteractionCollider.CenterXY).Position;
        _previewBuilding.IsPreview = true;

        OvertakeControl();
    }

    public void Hide()
    {
        if (!IsShowing) return;
        IsShowing = false;

        foreach (var item in UiElements)
        {
            item.IsShowing = IsShowing;
        }

        _previewBuilding?.TDObject.Destroy();
        ReturnControl();
    }

    private void OvertakeControl()
    {
        if (_activeControl) return;
        _activeControl = true;

        Player.PartialActiveInput = false;
        _cooldown = _cooldownDuration;
    }

    private void ReturnControl()
    {
        Player.PartialActiveInput = true;
        _activeControl = false;

        Player.Cooldown = _cooldownDuration;
        Player.SetProgressForCooldown();
    }

    private void ShowNextBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)CurrentBuilding == selectableBuildingCount - 1) CurrentBuilding = 0;
        else CurrentBuilding++;

        SetDescription(CurrentBuilding);

        _previewBuilding.TDObject.Destroy();
        _previewBuilding = GetBuilding(CurrentBuilding);
        _previewBuilding.IsPreview = true;

        _cooldown = _cooldownDuration;
    }

    private void ShowPreviousBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)CurrentBuilding == 0) CurrentBuilding = (SelectableBuilding)selectableBuildingCount - 1;
        else CurrentBuilding--;

        SetDescription(CurrentBuilding);

        _previewBuilding.TDObject.Destroy();
        _previewBuilding = GetBuilding(CurrentBuilding);
        _previewBuilding.IsPreview = true;

        _cooldown = _cooldownDuration;
    }

    private void SetDescription(SelectableBuilding currentBuilding)
    {
        Icon.Texture = GetIcon(CurrentBuilding);
        Title.Text = GetTitle(CurrentBuilding);
        Title.TDObject.RectTransform.Origin = .5f * new Vector2(Title.Width, Title.Height);

        var cost = GetCost(CurrentBuilding);
        WoodCost.Text = cost.Wood.ToString();
        StoneCost.Text = cost.Stone.ToString();

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

    private string GetTitle(SelectableBuilding building)
    {
        return building switch
        {
            SelectableBuilding.ResourceBuilding => "Collector",
            _ => building.ToString(),
        };
    }

    private string GetDescription(SelectableBuilding building)
    {
        return building switch
        {
            SelectableBuilding.Outpost => $"Shoots at enemies in range. \nConsumes {Config.OUTPOST_FOOD_UPKEEP} food/s.",
            SelectableBuilding.Wall => "Stops enemies from passing through.",
            SelectableBuilding.Farm => $"Needed to sustain outposts. \nProduces {Config.FARM_FOOD_UPKEEP} food/s",
            SelectableBuilding.Bridge => "Allows both players and enemies to cross bodies of water.",
            SelectableBuilding.Hospital => "Continously heals nearby players.",
            SelectableBuilding.ResourceBuilding => "Automatically collects nearby resources.",
            _ => throw new NotImplementedException(),
        };
    }

    private void InitializeCosts()
    {
        foreach (SelectableBuilding building in Enum.GetValues(typeof(SelectableBuilding)))
        {
            _costs[building] = building switch
            {
                SelectableBuilding.Outpost => new ResourcePile(Config.OUTPOST_WOOD_COST, Config.OUTPOST_STONE_COST, Config.OUTPOST_FOOD_UPKEEP),
                SelectableBuilding.Wall => new ResourcePile(Config.WALL_WOOD_COST, Config.WALL_STONE_COST),
                SelectableBuilding.Farm => new ResourcePile(Config.FARM_WOOD_COST, Config.FARM_STONE_COST, Config.FARM_FOOD_UPKEEP),
                SelectableBuilding.Bridge => new ResourcePile(Config.BRIDGE_WOOD_COST, Config.BRIDGE_STONE_COST),
                SelectableBuilding.Hospital => new ResourcePile(Config.HOSPITAL_WOOD_COST, Config.HOSPITAL_STONE_COST),
                SelectableBuilding.ResourceBuilding => new ResourcePile(Config.RESOURCE_BUILDING_WOOD_COST, Config.RESOURCE_BUILDING_STONE_COST),
                _ => throw new NotImplementedException(),
            };
        }
    }

    private ResourcePile GetCost(SelectableBuilding building)
    {
        return _costs[building];
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
