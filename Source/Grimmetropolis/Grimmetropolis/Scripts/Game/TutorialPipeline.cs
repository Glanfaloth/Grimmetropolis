using Microsoft.Xna.Framework;

using System.Collections.Generic;

public enum TutorialState
{
    ShowIntroductionText,
    ShowingIntroductionText,
    ShowIntroduction2Text,
    ShowingIntroduction2Text,
    WaitingForAxe,
    ShowAxeText,
    ShowingAxeText,
    WaitingForHarvestWood,
    ShowStoneText,
    ShowingStoneText,
    WaitingForPickaxe,
    WaitingForHarvestStone,
    ShowHammerText,
    ShowingHammerText,
    WaitingForHammer,
    WaitingForOutpostBuildMenu,
    WaitingForOutpostIconBuildMenu,
    WaitingForPlaceOutpost,
    ShowOutpostText,
    ShowingOutpostText,
    WaitingForBuildOutpost,
    ShowBuildOutpostText,
    ShowingBuildOutpostText,
    ShowFarmText,
    ShowingFarmText,
    WaitingForFarmBuildMenu,
    WaitingForFarmIconBuildMenu,
    WaitingForPlaceFarm,
    WaitingForBuildFarm,
    ShowRestText,
    ShowingRestText,
    ShowRest2Text,
    ShowingRest2Text,
    Ending
}

public class TutorialPipeline : TDComponent
{

private string _introductionText =
@"Welcome to the world of Grimmetropolis!

You are chosen to be the protector of the magical artifact.
You see this beautiful artifact over the castle?

Such magnificiency!";

private string _introduction2Text =
@"Oh my god, I can see enemies in the far distance!

We have to setup a defense. Quick, get me an axe!";

private string _axeText =
@"You see the small forests around the world? Chop some
trees to get some wood. We will need several wood logs
to build up some defenses.";

private string _stoneText =
@"Besides wood, stone is also a very important resource.
Collect some stone with a pickaxe.";

private string _hammerText =
@"There are a lot of different buildings you can construct.
Let's start with an outpost. It will shoot down close by
enemies in a circle of around two map tiles";

private string _placeOutpostText =
@"Now, that you have placed the building, it needs to be
constructed. Use the hammer on the building. Besides
that, you can also use the hammer to repair damaged
buildings.";

private string _buildOutpostText =
@"The outpost is the only building which requires
an upkeep of one food resource per second. If you
cannot deliver the food, the shooting distance
and rate will be halfed.";

private string _buildFarmText =
@"Therefore, it is always important to keep a close
eye on your food income. Food can be collected from
a farm.

As a next task, build a farm!";

private string _restingText =
@"Well done!

The castle will protect the magical artifact as long as the
castle is intact. If the castle is almost destroyed, the
enemies are able to get the magical artifact. Your last
resort is then to attack that thief and bring it back to
the castle.";

private string _resting2Text =
@"Good luck with your defence! May the gods help you!";


    private TutorialState _tutorialState = TutorialState.ShowIntroductionText;

    private List<ButtonIcon> _buttonIcons = new List<ButtonIcon>();

    private float _time = 1f;

    private ResourcePile _temporaryResourcePile = new ResourcePile();

    public override void Initialize()
    {
        base.Initialize();

        if (GameManager.TutorialFinished)
        {
            _tutorialState = TutorialState.Ending;
            return;
        }

        _temporaryResourcePile = GameManager.Instance.ResourcePool;
        GameManager.Instance.ResourcePool = new ResourcePile();
        GameManager.Instance.EnemyController.IsActive = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_time <= 0f)
        {
            switch (_tutorialState)
            {
                case TutorialState.ShowIntroductionText:
                    UIManager.Instance.SpeechBubble.Show(_introductionText);
                    _tutorialState = TutorialState.ShowingIntroductionText;
                    break;
                case TutorialState.ShowingIntroductionText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing)
                    {
                        _tutorialState = TutorialState.ShowIntroduction2Text;
                    }
                    break;
                case TutorialState.ShowIntroduction2Text:
                    UIManager.Instance.SpeechBubble.Show(_introduction2Text);
                    _tutorialState = TutorialState.ShowingIntroduction2Text;
                    break;
                case TutorialState.ShowingIntroduction2Text:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForAxe;
                    break;
                case TutorialState.WaitingForAxe:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Item item in GameManager.Instance.Items)
                        {
                            if (item is ToolAxe) AddButtonIcon(item.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolAxe)
                            {
                                _tutorialState = TutorialState.ShowAxeText;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.ShowAxeText:
                    UIManager.Instance.SpeechBubble.Show(_axeText);
                    _tutorialState = TutorialState.ShowingAxeText;
                    break;
                case TutorialState.ShowingAxeText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForHarvestWood;
                    break;
                case TutorialState.WaitingForHarvestWood:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is ResourceDeposit resourceDeposit && resourceDeposit.Type == ResourceDepositType.Wood)
                                AddButtonIcon(structure.TDObject.Transform, ButtonType.Action, 1.5f * Vector3.Backward);
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.ResourcePool.Wood >= 9)
                        {
                            _tutorialState = TutorialState.ShowStoneText;
                            DestroyButtonIcons();
                        }
                    }
                    break;
                case TutorialState.ShowStoneText:
                    UIManager.Instance.SpeechBubble.Show(_stoneText);
                    _tutorialState = TutorialState.ShowingStoneText;
                    break;
                case TutorialState.ShowingStoneText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForPickaxe;
                    break;
                case TutorialState.WaitingForPickaxe:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Item item in GameManager.Instance.Items)
                        {
                            if (item is ToolPickaxe) AddButtonIcon(item.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolPickaxe)
                            {
                                _tutorialState = TutorialState.WaitingForHarvestStone;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForHarvestStone:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is ResourceDeposit resourceDeposit && resourceDeposit.Type == ResourceDepositType.Stone)
                                AddButtonIcon(structure.TDObject.Transform, ButtonType.Action, 1.5f * Vector3.Backward);
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.ResourcePool.Stone >= 7)
                        {
                            _tutorialState = TutorialState.ShowHammerText;
                            DestroyButtonIcons();
                        }
                    }
                    break;
                case TutorialState.ShowHammerText:
                    UIManager.Instance.SpeechBubble.Show(_hammerText);
                    _tutorialState = TutorialState.ShowingHammerText;
                    break;
                case TutorialState.ShowingHammerText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForHammer;
                    break;
                case TutorialState.WaitingForHammer:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Item item in GameManager.Instance.Items)
                        {
                            if (item is ToolHammer) AddButtonIcon(item.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer)
                            {
                                _tutorialState = TutorialState.WaitingForOutpostBuildMenu;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForOutpostBuildMenu:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.BuildMenu, 2 * Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.BuildMenu.IsShowing)
                            {
                                _tutorialState = TutorialState.WaitingForOutpostIconBuildMenu;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForOutpostIconBuildMenu:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.CycleRight, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.BuildMenu.CurrentBuilding == SelectableBuilding.Outpost)
                            {
                                _tutorialState = TutorialState.WaitingForPlaceOutpost;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForPlaceOutpost:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Outpost outpost && outpost.IsPlaced)
                            {
                                _tutorialState = TutorialState.ShowOutpostText;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.ShowOutpostText:
                    UIManager.Instance.SpeechBubble.Show(_placeOutpostText);
                    _tutorialState = TutorialState.ShowingOutpostText;
                    break;
                case TutorialState.ShowingOutpostText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForBuildOutpost;
                    break;
                case TutorialState.WaitingForBuildOutpost:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Outpost outpost && outpost.IsBlueprint) AddButtonIcon(structure.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Outpost outpost && !outpost.IsBlueprint)
                            {
                                _tutorialState = TutorialState.ShowBuildOutpostText;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.ShowBuildOutpostText:
                    UIManager.Instance.SpeechBubble.Show(_buildOutpostText);
                    _tutorialState = TutorialState.ShowingBuildOutpostText;
                    break;
                case TutorialState.ShowingBuildOutpostText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.ShowFarmText;
                    break;
                case TutorialState.ShowFarmText:
                    UIManager.Instance.SpeechBubble.Show(_buildFarmText);
                    _tutorialState = TutorialState.ShowingFarmText;
                    break;
                case TutorialState.ShowingFarmText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing) _tutorialState = TutorialState.WaitingForFarmBuildMenu;
                    break;
                case TutorialState.WaitingForFarmBuildMenu:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.BuildMenu, 2 * Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.BuildMenu.IsShowing)
                            {
                                _tutorialState = TutorialState.WaitingForFarmIconBuildMenu;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForFarmIconBuildMenu:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.CycleRight, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.BuildMenu.CurrentBuilding == SelectableBuilding.Farm)
                            {
                                _tutorialState = TutorialState.WaitingForPlaceFarm;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForPlaceFarm:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Player player in GameManager.Instance.Players)
                        {
                            if (player.Items[0] is ToolHammer) AddButtonIcon(player.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Farm farm && farm.IsPlaced)
                            {
                                _tutorialState = TutorialState.WaitingForBuildFarm;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.WaitingForBuildFarm:
                    if (_buttonIcons.Count <= 0)
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Farm farm && farm.IsBlueprint) AddButtonIcon(structure.TDObject.Transform, ButtonType.Action, Vector3.Backward);
                        }
                    }
                    else
                    {
                        foreach (Structure structure in GameManager.Instance.Structures)
                        {
                            if (structure is Farm farm && !farm.IsBlueprint)
                            {
                                _tutorialState = TutorialState.ShowRestText;
                                DestroyButtonIcons();
                            }
                        }
                    }
                    break;
                case TutorialState.ShowRestText:
                    UIManager.Instance.SpeechBubble.Show(_restingText);
                    _tutorialState = TutorialState.ShowingRestText;
                    break;
                case TutorialState.ShowingRestText:
                    if (!UIManager.Instance.SpeechBubble.IsShowing)
                    {
                        _tutorialState = TutorialState.ShowRest2Text;
                    }
                    break;
                case TutorialState.ShowRest2Text:
                    UIManager.Instance.SpeechBubble.Show(_resting2Text);
                    _tutorialState = TutorialState.ShowingRest2Text;
                    break;
                case TutorialState.ShowingRest2Text:
                    if (!UIManager.Instance.SpeechBubble.IsShowing)
                    {
                        _tutorialState = TutorialState.Ending;
                        EndTutorial();
                    }
                    break;
            }
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        DestroyButtonIcons();
        EndTutorial();
    }

    private void EndTutorial()
    {
        GameManager.TutorialFinished = true;
        GameManager.Instance.ResourcePool = _temporaryResourcePile;
        GameManager.Instance.EnemyController.IsActive = true;

        /*for (int i = GameManager.Instance.Structures.Count - 1; i >= 0; i--)
        {
            Structure structure = GameManager.Instance.Structures[i];
            if (!(structure is ResourceDeposit || structure is Outpost || structure is Farm || structure is Castle))
                structure.Destroy();
        }*/
    }

    private void AddButtonIcon(TDTransform transform, ButtonType buttonType, Vector3 offset)
    {
        TDObject buttonIconObject = PrefabFactory.CreatePrefab(PrefabType.ButtonIcon, transform);
        buttonIconObject.RectTransform.Offset = offset;

        ButtonIcon buttonIcon = buttonIconObject.GetComponent<ButtonIcon>();
        buttonIcon.ButtonType = buttonType;

        _buttonIcons.Add(buttonIcon);
    }

    private void DestroyButtonIcons()
    {
        for (int i = _buttonIcons.Count - 1; i >= 0; i--)
        {
            _buttonIcons[i].TDObject?.Destroy();
            _buttonIcons.RemoveAt(i);
        }
    }
}