using Microsoft.Xna.Framework;

public class ReviveMenu : TDComponent
{
    public TDSprite Background;
    public TDText Title;
    public TDSprite WoodIcon;
    public TDSprite StoneIcon;
    public TDSprite FoodIcon;
    public TDText WoodCost;
    public TDText StoneCost;
    public TDText FoodCost;

    public bool IsShowing = true;
    private float _time = 1f;
    private float _showTime = 1f;

    public override void Initialize()
    {
        base.Initialize();

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (IsShowing && _time <= 0f) Hide();
    }

    public void Show()
    {
        _time = _showTime;

        if (IsShowing) return;
        IsShowing = true;

        Background.IsShowing = true;
        Title.IsShowing = true;
        WoodIcon.IsShowing = true;
        StoneIcon.IsShowing = true;
        FoodIcon.IsShowing = true;
        WoodCost.IsShowing = true;
        StoneCost.IsShowing = true;
        FoodCost.IsShowing = true;
    }

    public void Hide()
    {
        if (!IsShowing) return;
        IsShowing = false;

        Background.IsShowing = false;
        Title.IsShowing = false;
        WoodIcon.IsShowing = false;
        StoneIcon.IsShowing = false;
        FoodIcon.IsShowing = false;
        WoodCost.IsShowing = false;
        StoneCost.IsShowing = false;
        FoodCost.IsShowing = false;
    }
}