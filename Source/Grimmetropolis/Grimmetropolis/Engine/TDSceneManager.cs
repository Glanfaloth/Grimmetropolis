using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class TDSceneManager
{
	public static GraphicsDeviceManager Graphics;
	public static SpriteBatch SpriteBatch;
	public static TDScene ActiveScene;

	private static TDScene _temporaryScene;

	public static void LoadScene(TDScene scene)
	{
		if (ActiveScene == null)
        {
			ActiveScene = scene;
			ActiveScene.Initialize();
        }
		else
        {
			ActiveScene.RequiresLoadingScene = true;
			_temporaryScene = scene;
        }

	}

	public static void LoadTemporaryScene()
    {
		ActiveScene = _temporaryScene;
		ActiveScene.Initialize();
    }
}
