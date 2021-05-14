using Microsoft.Xna.Framework;

public class GameOverOverlay : TDComponent
{
    public TDSprite BlackOverlay;

    public TDText GameOverText;
    public TDText SurvivalTimeText;
    public TDText RestartText;

    private float _playTime = 0f;

    private Vector2 _offsetSurvivalTimeText;

    private bool _isShowing = true;

    public override void Initialize()
    {
        base.Initialize();

        _offsetSurvivalTimeText = SurvivalTimeText.TDObject.RectTransform.LocalPosition;

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        switch (GameManager.Instance.GameState)
        {
            case GameState.Playing:
                _playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case GameState.GameOver:
                foreach (TDInput input in TDInputManager.Inputs)
                {
                    if (input.ActionPressed()) TDSceneManager.LoadScene(new GameScene());
                }
                break;
        }
    }

    public void Show()
    {
        if (_isShowing) return;
        _isShowing = true;

        SurvivalTimeText.Text = $"You survived {(int)_playTime / 60} minutes and {(int)_playTime % 60} seconds!";
        SurvivalTimeText.TDObject.RectTransform.Origin = .5f * new Vector2(SurvivalTimeText.Width, SurvivalTimeText.Height);
        SurvivalTimeText.TDObject.RectTransform.LocalPosition = _offsetSurvivalTimeText;

        BlackOverlay.IsShowing = true;
        GameOverText.IsShowing = true;
        SurvivalTimeText.IsShowing = true;
        RestartText.IsShowing = true;
    }

    public void Hide()
    {
        if (!_isShowing) return;
        _isShowing = false;

        BlackOverlay.IsShowing = false;
        GameOverText.IsShowing = false;
        SurvivalTimeText.IsShowing = false;
        RestartText.IsShowing = false;
    }
}