using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum ButtonType
{
    Move,
    Action,
    Cancel,
    CycleLeft,
    CycleRight,
    BuildMenu
}

public class ButtonIcon : TDComponent
{
    public TDSprite Icon;

    public ButtonType ButtonType = ButtonType.Action;

    public override void Initialize()
    {
        base.Initialize();

        Icon.Texture = GetButtonIconFromButtonType();
        TDObject.RectTransform.Origin = .5f * new Vector2(Icon.Texture.Width, Icon.Texture.Height);
    }

    private Texture2D GetButtonIconFromButtonType()
    {
        return ButtonType switch
        {
            ButtonType.Move => TDContentManager.LoadTexture("UIXBoxLStick"),
            ButtonType.Action => TDContentManager.LoadTexture("UIXboxA"),
            ButtonType.Cancel => TDContentManager.LoadTexture("UIXboxB"),
            ButtonType.CycleLeft => TDContentManager.LoadTexture("UIXboxDpadLeft"),
            ButtonType.CycleRight => TDContentManager.LoadTexture("UIXboxDpadRight"),
            ButtonType.BuildMenu => TDContentManager.LoadTexture("UIXboxRB"),
            _ => TDContentManager.LoadTexture("UIXboxA")
        };
    }
}