using Microsoft.Xna.Framework;

using System;

public class WaveIndicator : TDComponent
{
    public TDSprite WarningSign;
    public WaveBar WaveCountDown = null;
    public TDSprite WaveBarDecoration;

    private bool _isShowingWarningSign = true;
    public bool IsFirstWave = true;

    public override void Initialize()
    {
        base.Initialize();

        WaveCountDown.Initialize();

        // WarningSign.TDObject.RectTransform.Parent3D = GameManager.Instance.Map.MapTiles[10, 10].TDObject.Transform;
        HideWarningSign();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsFirstWave)
        {
            WaveCountDown.MaxProgress = Config.TIME_UNTIL_FIRST_WAVE;
            WaveCountDown.CurrentProgress = Config.TIME_UNTIL_FIRST_WAVE - GameManager.Instance.EnemyController._waveTimer;
        }
        else
        {
            WaveCountDown.MaxProgress = Config.TIME_BETWEEN_WAVES;
            WaveCountDown.CurrentProgress = Config.TIME_BETWEEN_WAVES - GameManager.Instance.EnemyController._waveTimer;
        }

        WaveCountDown.Show();
    }

    public void ShowWarningSign()
    {
        if (_isShowingWarningSign) return;
        _isShowingWarningSign = true;
        WarningSign.TDObject.RectTransform.Parent3D = GameManager.Instance.EnemyController.StartTile.TDObject.Transform;
        Vector3 startPosition = .2f * Vector3.Backward;
        Vector3 endPosition = 1.2f * Vector3.Backward;
        WarningSign.TDObject.RunAction(1f, (p) =>
        {
            WarningSign.TDObject.RectTransform.Offset = Vector3.Lerp(startPosition, endPosition, .5f + .5f * MathF.Sin(MathHelper.TwoPi * p));
        }, true);
        WarningSign.IsShowing = true;
    }

    public void HideWarningSign()
    {
        if (!_isShowingWarningSign) return;
        _isShowingWarningSign = false;

        WarningSign.IsShowing = false;
    }
}
