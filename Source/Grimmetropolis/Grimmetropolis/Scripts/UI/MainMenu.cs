using Microsoft.Xna.Framework;

using System.Linq;

public enum MainMenuState
{
    MenuInactive,
    MenuStart,
    MenuSettings,
    SoundBar
}

public class MainMenu : TDComponent
{
    public TDSprite SplashScreen;
    public TDText SplashScreenText;
    public TDSprite SplashScreenButton;

    public TDSprite GameLogo;

    public TDSprite StartButton;
    public TDSprite SettingsButton;
    public TDSprite SoundIcon;
    public TDSprite SoundBarFront;
    public TDSprite SoundBarBack;
    public TDSprite SoundBar;
    public TDText GoBackText;
    public TDSprite GoBackButton;

    public CharacterSelectionDisplay[] CharacterDisplays = new CharacterSelectionDisplay[4];

    private bool _showSplashScreen = true;

    private float _cooldown = 0f;
    private float _cooldownDuration = .5f;

    private float _widthScaleSoundBar = 0f;

    private MainMenuState _mainMenuState = MainMenuState.MenuInactive;
    public MainMenuState MainMenuState
    {
        get => _mainMenuState;
        set
        {
            _mainMenuState = value;
            UpdateMenuState();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        _widthScaleSoundBar = SoundBar.TDObject.RectTransform.LocalScale.X;
        UpdateGeneralSound(0f);
        UpdateMenuState();

        for (int i = 0; i < CharacterDisplays.Length; i++)
        {
            CharacterDisplays[i] = PrefabFactory.CreatePrefab(PrefabType.CharacterDisplay, TDObject.Transform).GetComponent<CharacterSelectionDisplay>();

            switch (i)
            {
                case 0: CharacterDisplays[i].TDObject.RectTransform.Position = new Vector2(100f, 100f); break;
                case 1: CharacterDisplays[i].TDObject.RectTransform.Position = new Vector2(100f, 700f); break;
                case 2: CharacterDisplays[i].TDObject.RectTransform.Position = new Vector2(1400f, 100f); break;
                case 3: CharacterDisplays[i].TDObject.RectTransform.Position = new Vector2(1400f, 700f); break;
            }
            CharacterDisplays[i].CharacterAnimation = MenuUIManager.Instance.CharacterAnimations[i];
        }

        GameManager.PlayerTypeIndices.Clear();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_cooldown > 0f) return;

        if (_showSplashScreen)
        {
            foreach (TDInput input in TDInputManager.Inputs)
            {
                if (input.ActionPressed())
                {
                    _showSplashScreen = false;
                    SplashScreen.IsShowing = false;
                    SplashScreenText.IsShowing = false;
                    SplashScreenButton.IsShowing = false;
                    _cooldown = _cooldownDuration;
                }
            }
        }
        else
        {
            foreach (TDInput input in TDInputManager.Inputs)
            {
                if (TDInputManager.PlayerInputs.Contains(input)) continue;

                if (input.ActionPressed())
                {
                    AddPlayer(input);
                    _cooldown = _cooldownDuration;
                }
            }
        }
    }

    public void AddPlayer(TDInput input)
    {
        for (int i = 0; i < GameManager.PlayerTypes.Length; i++)
        {
            if (GameManager.PlayerTypes[i] == PlayerType.None)
            {
                GameManager.PlayerTypes[i] = PlayerType.Cinderella;
                MenuUIManager.Instance.CharacterAnimations[i].IsShowing = true;
                TDInputManager.PlayerInputs.Add(input);
                GameManager.PlayerTypeIndices.Add(i);

                CharacterDisplays[i].SetCharacterDisplayState(CharacterDisplayState.SelectingCharacter);
                CharacterDisplays[i].Input = input;
                break;
            }
        }
    }

    public void RemovePlayer(TDInput input)
    {
        int index = -1;
        for (int i = 0; i < TDInputManager.PlayerInputs.Count; i++)
        {
            if (TDInputManager.PlayerInputs[i] == input)
            {
                index = GameManager.PlayerTypeIndices[i];
                break;
            }
        }
        GameManager.PlayerTypes[index] = PlayerType.None;
        MenuUIManager.Instance.CharacterAnimations[index].IsShowing = false;

        CharacterDisplays[index].SetCharacterDisplayState(CharacterDisplayState.NotUsed);
        CharacterDisplays[index].Input = null;

        TDInputManager.PlayerInputs.Remove(input);
        GameManager.PlayerTypeIndices.Remove(index);
    }

    public bool NavigateMainMenu(TDInput input, GameTime gameTime)
    {
        Vector2 joystickDirection = input.MoveDirection();

        switch (_mainMenuState)
        {
            case MainMenuState.MenuStart:
                if (input.ActionPressed())
                {
                    if (CharacterDisplays.All(o => o.CharacterDisplayState != CharacterDisplayState.SelectingCharacter))
                    {
                        for (int i = 0; i < GameManager.PlayerTypes.Length; i++)
                        {
                            GameManager.PlayerTypes[i] = CharacterDisplays[i].CharacterDisplayState == CharacterDisplayState.Ready
                                ? GameManager.PlayerTypes[i] = CharacterDisplays[i].CurrentPlayerType : PlayerType.None;
                        }
                        TDSceneManager.LoadScene(new GameScene());
                    }
                    return true;
                }
                if (joystickDirection.Y <= -.8f)
                {
                    MainMenuState = MainMenuState.MenuSettings;
                    return true;
                }
                break;
            case MainMenuState.MenuSettings:
                if (input.ActionPressed())
                {
                    MainMenuState = MainMenuState.SoundBar;
                    return true;
                }
                if (joystickDirection.Y >= .8f)
                {
                    MainMenuState = MainMenuState.MenuStart;
                    return true;
                }
                break;
            case MainMenuState.SoundBar:
                if (input.CancelPressed())
                {
                    MainMenuState = MainMenuState.MenuStart;
                    return true;
                }
                if (joystickDirection.X <= -.8f)
                {
                    UpdateGeneralSound(-.4f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    return false;
                }
                if (joystickDirection.X >= .8f)
                {
                    UpdateGeneralSound(.4f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    return false;
                }
                break;
        }

        return false;
    }

    void UpdateGeneralSound(float change)
    {
        Vector2 currentScale = SoundBar.TDObject.RectTransform.LocalScale;
        TDSound.Volume = MathHelper.Clamp(TDSound.Volume + change, 0f, 1f);
        currentScale.X = MathHelper.Lerp(0f, _widthScaleSoundBar, TDSound.Volume);
        SoundBar.TDObject.RectTransform.LocalScale = currentScale;

        if (TDSound.Volume <= 0f) SoundIcon.Texture = TDContentManager.LoadTexture("UIVolumeOff");
        else SoundIcon.Texture = TDContentManager.LoadTexture("UIVolumeOn");
    }

    private void UpdateMenuState()
    {
        switch (_mainMenuState)
        {
            case MainMenuState.MenuInactive:
                StartButton.IsShowing = true;
                StartButton.Color = .5f * Color.White;
                SettingsButton.IsShowing = true;
                SettingsButton.Color = .5f * Color.White;
                SoundIcon.IsShowing = false;
                SoundBarBack.IsShowing = false;
                SoundBarFront.IsShowing = false;
                SoundBar.IsShowing = false;
                GoBackText.IsShowing = false;
                GoBackButton.IsShowing = false;
                break;
            case MainMenuState.MenuStart:
                StartButton.IsShowing = true;
                StartButton.Color = Color.White;
                SettingsButton.IsShowing = true;
                SettingsButton.Color = .5f * Color.White;
                SoundIcon.IsShowing = false;
                SoundBarBack.IsShowing = false;
                SoundBarFront.IsShowing = false;
                SoundBar.IsShowing = false;
                GoBackText.IsShowing = false;
                GoBackButton.IsShowing = false;
                break;
            case MainMenuState.MenuSettings:
                StartButton.IsShowing = true;
                StartButton.Color = .5f * Color.White;
                SettingsButton.IsShowing = true;
                SettingsButton.Color = Color.White;
                SoundIcon.IsShowing = false;
                SoundBarBack.IsShowing = false;
                SoundBarFront.IsShowing = false;
                SoundBar.IsShowing = false;
                GoBackText.IsShowing = false;
                GoBackButton.IsShowing = false;
                break;
            case MainMenuState.SoundBar:
                StartButton.IsShowing = false;
                SettingsButton.IsShowing = false;
                SoundIcon.IsShowing = true;
                SoundBarBack.IsShowing = true;
                SoundBarFront.IsShowing = true;
                SoundBar.IsShowing = true;
                GoBackText.IsShowing = true;
                GoBackButton.IsShowing = true;
                break;
        }
    }
}