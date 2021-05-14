using Microsoft.Xna.Framework;

public class MainMenu : TDComponent
{

    public TDSprite GameLogo;

    public TDSprite StartButton;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (TDInput input in TDInputManager.Inputs)
        {
            if (input.ActionPressed()) TDSceneManager.LoadScene(new GameScene());
        }
    }
}