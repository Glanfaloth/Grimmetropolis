public class UIManager : TDComponent
{
    public static UIManager Instance;

    public ResourceDisplay ResourceDisplay;

    public override void Initialize()
    {
        base.Initialize();

        Instance = this;

        TDObject resourceDisplayObject = PrefabFactory.CreatePrefab(PrefabType.ResourceDisplay);
        ResourceDisplay = resourceDisplayObject.GetComponent<ResourceDisplay>();
    }
}
