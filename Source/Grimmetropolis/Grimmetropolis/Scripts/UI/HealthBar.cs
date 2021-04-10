using Microsoft.Xna.Framework;

using System.Diagnostics;

public class HealthBar : ProgressBar
{
    private float _revealTime = 3f;
    private float _time = 0f;

    public override void Initialize()
    {
        base.Initialize();

        SetHealthBar(CurrentProgress);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_time <= 0f) Hide();
    }

    public void SetHealthBar(float health)
    {
        _time = _revealTime;

        CurrentProgress = health;
        float proportion = CurrentProgress / MaxProgress;

        Show();

        if (proportion < .25f) Foreground.Color = Color.Red;
        else if (proportion < .5f) Foreground.Color = Color.Yellow;
        else Foreground.Color = Color.Green;
    }
}
