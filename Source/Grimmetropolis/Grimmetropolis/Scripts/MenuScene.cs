
public class MenuScene : TDScene
{
    public MenuScene() { }

    public override void Initialize()
    {
        base.Initialize();

        PrefabFactory.CreatePrefab(PrefabType.MenuUIManager);
    }
}
