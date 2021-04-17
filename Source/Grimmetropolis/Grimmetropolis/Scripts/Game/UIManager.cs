using Microsoft.Xna.Framework;

public class UIManager : TDComponent
{
    public static UIManager Instance;

    public ResourceDisplay ResourceDisplay;
    public PlayerDisplay[] PlayerDisplays = new PlayerDisplay[4];

    private int _playerDisplayIndex = 0;
    private float _offsetBetweenPlayerDisplay = .2f * TDSceneManager.Graphics.PreferredBackBufferWidth;

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        TDObject resourceDisplayObject = PrefabFactory.CreatePrefab(PrefabType.ResourceDisplay);
        ResourceDisplay = resourceDisplayObject.GetComponent<ResourceDisplay>();
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

        _playerDisplayIndex++;

        for (int i = 0; i < _playerDisplayIndex; i++)
        {
            PlayerDisplays[i].TDObject.RectTransform.LocalPosition = new Vector2(offsetStart + i * _offsetBetweenPlayerDisplay, TDSceneManager.Graphics.PreferredBackBufferHeight - 25f);
        }
    }
}
