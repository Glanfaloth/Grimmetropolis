using Microsoft.Xna.Framework;

using System.Diagnostics;

public class HealthBar : ProgressBar
{
    private float _revealTime = 3f;
    private float _time = 0f;

    private bool _quickShow = false;

    public override void Initialize()
    {
        base.Initialize();

        _time = 0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_time <= 0f) Hide();
    }

    public override void Show()
    {
        _time = _revealTime;

        base.Show();
    }

    public void QuickShow()
    {
        _quickShow = true;

        base.Show();
    }

    public override void Hide()
    {
        if (_quickShow) return;

        base.Hide();
    }

    public void QuickHide()
    {
        _quickShow = false;
        if (_time > 0f) return;

        base.Hide();
    }

    protected override void SetProgressBar()
    {
        base.SetProgressBar();

        float proportion = CurrentProgress / MaxProgress;

        if (proportion < .25f) Foreground.Color = Color.Red;
        else if (proportion < .5f) Foreground.Color = Color.Yellow;
        else Foreground.Color = Color.Green;
    }
}
