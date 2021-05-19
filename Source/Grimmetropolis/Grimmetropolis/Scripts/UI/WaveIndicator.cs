﻿using Microsoft.Xna.Framework;

using System;

public class WaveIndicator : TDComponent
{
    public TDSprite WarningSign;
    public TDText Text;
    public WaveBar WaveCountDown = null;

    private bool _isShowingWarningSign = true;
    private bool _isFirstWave = true;

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
    }

    public void ShowWarningSign()
    {
        if (_isShowingWarningSign) return;
        _isShowingWarningSign = true;
        WarningSign.TDObject.RectTransform.Parent3D = GameManager.Instance.EnemyController.StartTile.TDObject.Transform;

        WarningSign.IsShowing = true;
    }

    public void HideWarningSign()
    {
        if (!_isShowingWarningSign) return;
        _isShowingWarningSign = false;

        WarningSign.IsShowing = false;
    }
}
