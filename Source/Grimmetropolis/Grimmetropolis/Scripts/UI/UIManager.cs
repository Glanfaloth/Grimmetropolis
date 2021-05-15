using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class UIManager : TDComponent
{
    public static UIManager Instance;

    public ResourceDisplay ResourceDisplay;
    public WaveIndicator WaveIndicator;
    public PlayerDisplay[] PlayerDisplays = new PlayerDisplay[4];
    public GameOverOverlay GameOverOverlay;

    private int _playerDisplayIndex = 0;
    private int _playerItemDisplayIndex = 0;

    private float _offsetBetweenPlayerDisplay = .2f * TDSceneManager.Graphics.PreferredBackBufferWidth;

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
            PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIHammer");
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
            PlayerDisplays[i].PlayerIcon.Texture = GetPlayerIconFromPlayerType(GameManager.PlayerTypes[i]);
            PlayerDisplays[i].PlayerName.Text = GetNameFromPlayerType(GameManager.PlayerTypes[i]);
            //PlayerDisplays[i].CurrentItem = player.Items;
            PlayerDisplays[i].TDObject.RectTransform.LocalPosition = new Vector2(offsetStart + i * _offsetBetweenPlayerDisplay, TDSceneManager.Graphics.PreferredBackBufferHeight - 60f);
        }
    }

    public void UpdatePlayerDisplay(Player player)
    {
        if (_playerItemDisplayIndex >= PlayerDisplays.Length) _playerItemDisplayIndex = 0;
        if (GameManager.PlayerTypes[_playerDisplayIndex] != PlayerType.None)
        {
            if (player.Items[0] == null)
            {
                PlayerDisplays[_playerItemDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIPlayer");
            }
            else if (player.Items[0] is ToolAxe)
            {
                PlayerDisplays[_playerItemDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIAxe");
            }
            else if (player.Items[0] is ToolHammer)
            {
                PlayerDisplays[_playerItemDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIHammer");
            }
            else if (player.Items[0] is ToolPickaxe)
            {
                PlayerDisplays[_playerItemDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIPickaxe");
            }
            else
            {
                PlayerDisplays[_playerItemDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UISword");
            }
        }
        _playerItemDisplayIndex++;
    }
    public Texture2D GetPlayerIconFromPlayerType(PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Cinderella => TDContentManager.LoadTexture("UICinderella"),
            PlayerType.Snowwhite => TDContentManager.LoadTexture("UISnowWhite"),
            PlayerType.Frog => TDContentManager.LoadTexture("UIFrog"),
            PlayerType.Beast => TDContentManager.LoadTexture("UIBeast"),
            _ => TDContentManager.LoadTexture("UICinderella"),
        };
    }

    public string GetNameFromPlayerType(PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Cinderella => "Cinderella",
            PlayerType.Snowwhite => "Snow White",
            PlayerType.Frog => "Frog King",
            PlayerType.Beast => "The Beast",
            _ => "Cinderella"
        };
    }
}
