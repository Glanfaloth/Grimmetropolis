using Microsoft.Xna.Framework;

using System;

public class WaveIndicator : TDComponent
{
    public TDSprite Image;
    public TDText Text;
    public WaveBar WaveCountDown = null;

    protected bool _isShowing = false;
    protected bool _isFirstWave = true;

    public override void Initialize()
    {
        base.Initialize();

        /*
        Vector2 startPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 10f);
        Vector2 endPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 40f);

        TDObject.RunAction(1f, (p) =>
        {
            TDObject.RectTransform.Position = Vector2.Lerp(startPosition, endPosition, .5f + .5f * MathF.Sin(MathHelper.TwoPi * p));
        }, true);
        */

        WaveCountDown.Initialize();

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_isFirstWave)
        {
            WaveCountDown.MaxProgress = Config.TIME_UNTIL_FIRST_WAVE;
            WaveCountDown.CurrentProgress = Config.TIME_UNTIL_FIRST_WAVE - GameManager.Instance.EnemyController._waveTimer;
            _isFirstWave = false;
        }
        else
        {
            WaveCountDown.MaxProgress = Config.TIME_BETWEEN_WAVES;
            WaveCountDown.CurrentProgress = Config.TIME_BETWEEN_WAVES - GameManager.Instance.EnemyController._waveTimer;
        }

        WaveCountDown.Show();
        

        _isShowing = GameManager.Instance.EnemyController.WaveIndicator;

        if (_isShowing) Show();
        else if (!_isShowing) Hide();
    }

    public virtual void Show()
    {
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Image)) TDSceneManager.ActiveScene.SpriteObjects.Add(Image);
    }

    public virtual void Hide()
    {
        if (TDSceneManager.ActiveScene.SpriteObjects.Contains(Image)) TDSceneManager.ActiveScene.SpriteObjects.Remove(Image); 
    }
}
