using Microsoft.Xna.Framework;

using System.Diagnostics;

public class WaveBar : ProgressBar
{
    public bool AlwaysShow = false;

    private float _revealTime = 5f;
    private float _time = 0f;


    public override void Initialize()
    {
        base.Initialize();

        MaxWidth = 2.81f;

        _time = 0f;

        if (AlwaysShow)
        {
            Show();
            SetProgressBar();
        }

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

    public override void Hide()
    {
        if (AlwaysShow) return;

        base.Hide();
    }

    protected override void SetProgressBar()
    {
        base.SetProgressBar();

        float proportion = CurrentProgress / MaxProgress;

        if (proportion < .5f) Foreground.Color = Color.Green;
        else if (proportion < .75f) Foreground.Color = Color.Yellow;
        else Foreground.Color = Color.Red;
    }
}
