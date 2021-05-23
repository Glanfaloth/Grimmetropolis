using Microsoft.Xna.Framework;

public class ResourceDisplay : TDComponent
{
    public TDText WoodText;
    public TDText StoneText;
    public TDText FoodText;

    public override void Initialize()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        WoodText.Text = GameManager.Instance.ResourcePool.Wood.ToString();
        StoneText.Text = GameManager.Instance.ResourcePool.Stone.ToString();
        FoodText.Text = GameManager.Instance.ResourcePool.Food.ToString();

        WoodText.TDObject.RectTransform.Origin = new Vector2(WoodText.Width, WoodText.Height);
        StoneText.TDObject.RectTransform.Origin = new Vector2(StoneText.Width, StoneText.Height);
        FoodText.TDObject.RectTransform.Origin = new Vector2(FoodText.Width, FoodText.Height);
    }
}
