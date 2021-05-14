public class MenuUIManager : TDComponent
{
    public static MenuUIManager Instance;

    public override void Initialize()
    {
        Instance = this;

        PrefabFactory.CreatePrefab(PrefabType.MainMenu);
    }
}
