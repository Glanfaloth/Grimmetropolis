public class ResourceDisplay : TDComponent
{
    public TDText TextUI;

    public override void Initialize()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        TextUI.Text = GameManager.Instance.ResourcePool.ToString();
    }
}
