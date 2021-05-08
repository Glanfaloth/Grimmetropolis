using Microsoft.Xna.Framework;

public class ResourceDisplay : TDComponent
{
    public TDText TextUI;
    public TDSprite WoodUI;
    public TDSprite StoneUI;
    public TDSprite FoodUI;

    public override void Initialize()
    {
        UpdateDisplay();
        TDObject.RectTransform.Scale = 2f * Vector2.One;
        TDObject.RectTransform.LocalPosition = new Vector2(10f, 10f);
    }

    public void UpdateDisplay()
    {
        TextUI.Text = GameManager.Instance.ResourcePool.ToString();
    }
}
