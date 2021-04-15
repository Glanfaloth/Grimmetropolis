using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

public enum SelectableBuilding
{
    Outpost,
    Wall
}

public class BuildMenu : TDComponent
{
    public Player Player;

    public TDSprite BuildSprite;
    public TDSprite Icon;

    private bool _isShowing = true;
    private bool _requiresHide = false;

    private SelectableBuilding _currentBuilding;
    private bool _activeControl = true;

    private float _cooldown = 0f;
    private float _cooldownDuration = .2f;

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
            if (Player.Input.IsCycleNextItemPressed()) ShowNextBuilding();
            if (Player.Input.IsCyclePreviousItemPressed()) ShowPreviousBuilding();

            if (Player.Input.IsUseItemPressed()) Player.BuildBlueprint(GetBuilding(_currentBuilding));

            if (!Player.Input.IsSelectBuildingTypePressed()) Hide();
        }

        if (_requiresHide) Hide();
    }

    public void Show()
    {
        if (_isShowing) return;

        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(BuildSprite)) TDSceneManager.ActiveScene.SpriteObjects.Add(BuildSprite);
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Icon)) TDSceneManager.ActiveScene.SpriteObjects.Add(Icon);

        _isShowing = true;

        OvertakeControl();
    }

    public void Hide()
    {
        if (!_isShowing) return;

        if (TDSceneManager.ActiveScene.SpriteObjects.Contains(BuildSprite) && TDSceneManager.ActiveScene.SpriteObjects.Contains(Icon))
        {
            TDSceneManager.ActiveScene.SpriteObjects.Remove(BuildSprite);
            TDSceneManager.ActiveScene.SpriteObjects.Remove(Icon);

            _isShowing = false;
            _requiresHide = false;

            ReturnControl();
        }
        else _requiresHide = true;
    }

    private void OvertakeControl()
    {
        if (_activeControl) return;

        Player.ActiveInput = false;

        _activeControl = true;
    }

    private void ReturnControl()
    {
        Player.ActiveInput = true;

        _activeControl = false;
    }

    private void ShowNextBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)_currentBuilding == selectableBuildingCount - 1) _currentBuilding = 0;
        else _currentBuilding++;

        Icon.Texture = GetIcon(_currentBuilding);

        _cooldown = _cooldownDuration;
    }

    private void ShowPreviousBuilding()
    {
        int selectableBuildingCount = Enum.GetNames(typeof(SelectableBuilding)).Length;

        if ((int)_currentBuilding == 0) _currentBuilding = (SelectableBuilding)selectableBuildingCount - 1;
        else _currentBuilding--;

        Icon.Texture = GetIcon(_currentBuilding);
        _cooldown = _cooldownDuration;
    }

    private Texture2D GetIcon(SelectableBuilding buildingIcon)
    {
        if (Icon == null) return null;

        return buildingIcon switch
        {
            SelectableBuilding.Outpost => TDContentManager.LoadTexture("UIBuildingOutpostIcon"),
            SelectableBuilding.Wall => TDContentManager.LoadTexture("UIBuildingWallIcon"),
            _ => TDContentManager.LoadTexture("UIBuildingOutpostIcon")
        };
    }

    private Building GetBuilding(SelectableBuilding buildingIcon)
    {
        if (Icon == null) return null;

        return buildingIcon switch
        {
            SelectableBuilding.Outpost => PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost).GetComponent<Outpost>(),
            SelectableBuilding.Wall => PrefabFactory.CreatePrefab(PrefabType.BuildingWall).GetComponent<Wall>(),
            _ => PrefabFactory.CreatePrefab(PrefabType.BuildingOutpost).GetComponent<Outpost>()
        };
    }
}
