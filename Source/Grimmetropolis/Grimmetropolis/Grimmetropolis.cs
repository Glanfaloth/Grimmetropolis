using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Grimmetropolis : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Grimmetropolis()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        _graphics.PreferredBackBufferWidth *= 2;
        _graphics.PreferredBackBufferHeight *= 2;
        _graphics.ApplyChanges();

        Window.Title = "Grimmetropolis";
        TDSceneManager.LoadScene(new GameScene());
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        TDCamera.Graphics = _graphics;
        TDContentManager.Content = Content;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        TDSceneManager.ActiveScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        TDSceneManager.ActiveScene.Draw();

        base.Draw(gameTime);
    }
}