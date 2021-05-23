using Microsoft.Xna.Framework;

public class MainMenu : TDComponent
{
    public TDSprite SplashScreen;
    public TDText SplashScreenText;
    public TDSprite SplashScreenButton;

    public TDSprite GameLogo;

    public TDSprite StartButton;

    public CharacterSelectionDisplay[] CharacterDisplays = new CharacterSelectionDisplay[4];

    private bool _showSplashScreen = true;

    private float _cooldown = 0f;
    private float _cooldownDuration = .5f;

    public override void Initialize()
    {
        base.Initialize();

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
}