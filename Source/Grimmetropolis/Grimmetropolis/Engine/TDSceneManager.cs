public static class TDSceneManager
{
	public static TDScene ActiveScene;

	public static void LoadScene(TDScene scene)
	{
		ActiveScene = scene;
		ActiveScene.Initialize();
	}
}
