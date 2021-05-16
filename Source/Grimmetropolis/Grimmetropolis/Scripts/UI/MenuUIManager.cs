using Microsoft.Xna.Framework;

public class MenuUIManager : TDComponent
{
    public static MenuUIManager Instance;

    public MainMenu MainMenu;
    public CharacterAnimation[] CharacterAnimations = new CharacterAnimation[4];

    public override void Initialize()
    {
        Instance = this;

        MainMenu = PrefabFactory.CreatePrefab(PrefabType.MainMenu).GetComponent<MainMenu>();

        CharacterAnimation player0 = PrefabFactory.CreatePrefab(PrefabType.PlayerPreview, new Vector3(0f, -4.4f, 1.5f), Quaternion.Identity).GetComponent<CharacterAnimation>();
        player0.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
        player0.IsShowing = false;

        CharacterAnimation player1 = PrefabFactory.CreatePrefab(PrefabType.PlayerPreview, new Vector3(0f, -4.4f, -3.25f), Quaternion.Identity).GetComponent<CharacterAnimation>();
        player1.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
        player1.IsShowing = false;

        CharacterAnimation player2 = PrefabFactory.CreatePrefab(PrefabType.PlayerPreview, new Vector3(0f, 4.4f, 1.5f), Quaternion.Identity).GetComponent<CharacterAnimation>();
        player2.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
        player2.IsShowing = false;

        CharacterAnimation player3 = PrefabFactory.CreatePrefab(PrefabType.PlayerPreview, new Vector3(0f, 4.4f, -3.25f), Quaternion.Identity).GetComponent<CharacterAnimation>();
        player3.CharacterModel = TDContentManager.LoadModel("PlayerCindarella");
        player3.IsShowing = false;

        CharacterAnimations[0] = player0;
        CharacterAnimations[1] = player1;
        CharacterAnimations[2] = player2;
        CharacterAnimations[3] = player3;
    }
}
