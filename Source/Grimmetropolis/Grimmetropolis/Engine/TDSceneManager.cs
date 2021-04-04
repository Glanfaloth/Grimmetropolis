using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class TDSceneManager
{
	public static GraphicsDeviceManager Graphics;
	public static SpriteBatch SpriteBatch;
	public static TDScene ActiveScene;

	public static void LoadScene(TDScene scene)
	{
		ActiveScene = scene;
		ActiveScene.Initialize();
	}
}
