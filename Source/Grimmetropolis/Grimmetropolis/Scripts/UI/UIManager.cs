using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class UIManager : TDComponent
{
    public static UIManager Instance;

    public ResourceDisplay ResourceDisplay;
    public WaveIndicator WaveIndicator;
    public PlayerDisplay[] PlayerDisplays = new PlayerDisplay[4];
    public GameOverOverlay GameOverOverlay;
    public SpeechBubble SpeechBubble;

    private int _playerDisplayIndex = 0;
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

        TDObject speechBubbleObject = PrefabFactory.CreatePrefab(PrefabType.SpeechBubble);
        SpeechBubble = speechBubbleObject.GetComponent<SpeechBubble>();
    }

    public void ShowGameOver()
    {
        GameOverOverlay.Show();
    }

    public PlayerDisplay AddPlayerDisplay(Player player)
    {
        TDObject playerDisplayObject = PrefabFactory.CreatePrefab(PrefabType.PlayerDisplay);
        PlayerDisplays[_playerDisplayIndex] = playerDisplayObject.GetComponent<PlayerDisplay>();
        float offsetAmount = _playerDisplayIndex * _offsetBetweenPlayerDisplay;
        float offsetStart = .5f * (TDSceneManager.Graphics.PreferredBackBufferWidth - offsetAmount);

        player.HealthBar = PlayerDisplays[_playerDisplayIndex].HealthBar;
        PlayerDisplays[_playerDisplayIndex].HealthBar.CurrentProgress = player.Health;
        PlayerDisplays[_playerDisplayIndex].HealthBar.MaxProgress = player.BaseHealth;
        PlayerDisplays[_playerDisplayIndex].CurrentItem.Texture = TDContentManager.LoadTexture("UIAxe");

        PlayerDisplays[_playerDisplayIndex].PlayerType = player.PlayerType;

        _playerDisplayIndex++;

        for (int i = 0; i < _playerDisplayIndex; i++)
        {
            PlayerDisplays[i].TDObject.RectTransform.LocalPosition = new Vector2(offsetStart + i * _offsetBetweenPlayerDisplay, TDSceneManager.Graphics.PreferredBackBufferHeight - 60f);
        }

        return PlayerDisplays[_playerDisplayIndex - 1];
    }

    internal void ReusePlayerDisplay(PlayerDisplay display, Player player)
    {
        player.HealthBar = display.HealthBar;
    }
}
