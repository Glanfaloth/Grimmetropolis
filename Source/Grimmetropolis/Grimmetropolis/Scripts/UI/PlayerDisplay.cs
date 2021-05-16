using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PlayerDisplay : TDComponent {

    public HealthBar HealthBar = null;
    public TDSprite PlayerIcon;
    public TDText PlayerName;
    public TDSprite CurrentItem;

    public PlayerType PlayerType = PlayerType.None;

    public override void Initialize()
    {
        base.Initialize();

        PlayerIcon.Texture = GetPlayerIconFromPlayerType(PlayerType);
        PlayerName.Text = GetNameFromPlayerType(PlayerType);

        SetItemSprite(null);
    }

    public void SetItemSprite(Item item)
    {
        switch (item)
        {
            case ToolAxe _:
                CurrentItem.IsShowing = true;
                CurrentItem.Texture = TDContentManager.LoadTexture("UIAxe");
                break;
            case ToolPickaxe _:
                CurrentItem.IsShowing = true;
                CurrentItem.Texture = TDContentManager.LoadTexture("UIPickaxe");
                break;
            case ToolHammer _:
                CurrentItem.IsShowing = true;
                CurrentItem.Texture = TDContentManager.LoadTexture("UIHammer");
                break;
            case WeaponSword _:
                CurrentItem.IsShowing = true;
                CurrentItem.Texture = TDContentManager.LoadTexture("UISword");
                break;
            default:
                CurrentItem.IsShowing = false;
                break;
        }
    }

    private Texture2D GetPlayerIconFromPlayerType(PlayerType playerType)
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

    private string GetNameFromPlayerType(PlayerType playerType)
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
