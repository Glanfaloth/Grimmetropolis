using Microsoft.Xna.Framework;

using System.Diagnostics;

public class HealthBar : ProgressBar
{
    public bool AlwaysShow = false;

    private float _revealTime = 3f;
    private float _time = 0f;

    private bool _quickShow = false;

    public override void Initialize()
    {
        base.Initialize();

        _time = 0f;

        if (AlwaysShow)
            Show();

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!AlwaysShow)
        {
            _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time <= 0f) Hide();
        }
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
        if (_quickShow || AlwaysShow) return;

        base.Hide();
    }

    public void QuickHide()
    {
        if (AlwaysShow) return;

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
