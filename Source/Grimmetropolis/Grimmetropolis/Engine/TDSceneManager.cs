using Microsoft.Xna.Framework;

public static class TDSceneManager
{
	public static GraphicsDeviceManager Graphics;
	public static TDScene ActiveScene;

	public static void LoadScene(TDScene scene)
	{
		ActiveScene = scene;
		ActiveScene.Initialize();
	}
}
