using Microsoft.Xna.Framework;

public class UIManager : TDComponent
{
    public static UIManager Instance;

    public ResourceDisplay ResourceDisplay;
    public WaveIndicator WaveIndicator;
    public PlayerDisplay[] PlayerDisplays = new PlayerDisplay[4];
    public GameOverOverlay GameOverOverlay;

    private int _playerDisplayIndex = 0;
    private float _offsetBetweenPlayerDisplay = .2f * TDSceneManager.Graphics.PreferredBackBufferWidth;
    private string[] _playerIcons = { "UICinderella", "UISnowWhite", "UIFrog", "UIBeast" };
    private string[] _playerNames = { "Cinderella", "Snow White", "Frog King", "The Beast" };

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        TDObject resourceDisplayObject = PrefabFactory.CreatePrefab(PrefabType.ResourceDisplay);
        ResourceDisplay = resourceDisplayObject.GetComponent<ResourceDisplay>();

        TDObject waveIndicatorObject = PrefabFactory.CreatePrefab(PrefabType.WaveIndicator);
        WaveIndicator = waveIndicatorObject.GetComponent<WaveIndicator>();

        TDObject gameOverObject = PrefabFactory.CreatePrefab(PrefabType.GameOverOverlay);
        GameOverOverlay = gameOverObject.GetComponent<GameOverOverlay>();
    }

    public void ShowGameOver()
    {
        GameOverOverlay.Show();
    }

    public void AddPlayerDisplay(Player player)
    {
        if (_playerDisplayIndex >= PlayerDisplays.Length) return;

        TDObject playerDisplayObject = PrefabFactory.CreatePrefab(PrefabType.PlayerDisplay);
        PlayerDisplays[_playerDisplayIndex] = playerDisplayObject.GetComponent<PlayerDisplay>();
        float offsetAmount = _playerDisplayIndex * _offsetBetweenPlayerDisplay;
        float offsetStart = .5f * (TDSceneManager.Graphics.PreferredBackBufferWidth - offsetAmount);

        player.HealthBar = PlayerDisplays[_playerDisplayIndex].HealthBar;
        PlayerDisplays[_playerDisplayIndex].HealthBar.CurrentProgress = player.Health;
        PlayerDisplays[_playerDisplayIndex].HealthBar.MaxProgress = player.BaseHealth;

        if (player.Items[0] == null)
        {
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIPlayer");
        }
        else if (player.Items[0] is ToolAxe)
        {
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIAxe");
        }
        else if (player.Items[0] is ToolHammer)
        {
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIAxe");
        }
        else if (player.Items[0] is ToolPickaxe)
        {
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIPickaxe");
        }
        else
        {
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UISword");
        }

        _playerDisplayIndex++;

        for (int i = 0; i < _playerDisplayIndex; i++)
        {
            PlayerDisplays[i].PlayerIcon.Texture = TDContentManager.LoadTexture(_playerIcons[i]);
            PlayerDisplays[i].PlayerName.Text = _playerNames[i];
            //PlayerDisplays[i].CurrentItem = player.Items;
            PlayerDisplays[i].TDObject.RectTransform.LocalPosition = new Vector2(offsetStart + i * _offsetBetweenPlayerDisplay, TDSceneManager.Graphics.PreferredBackBufferHeight - 60f);
        }
    }
}
