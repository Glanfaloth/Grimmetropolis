using Microsoft.Xna.Framework;

using System.Diagnostics;

public class HealthBar : ProgressBar
{
    public float Health = 1f;
    public float BaseHealth = 1f;

    private float _revealTime = 3f;
    private float _time = 0f;

    public override void Initialize()
    {
        base.Initialize();

        SetHealthBar(Health);
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

        Health = health;
        Proportion = Health / BaseHealth;

        Show();

        if (Proportion < .25f) Foreground.Color = Color.Red;
        else if (Proportion < .5f) Foreground.Color = Color.Yellow;
        else Foreground.Color = Color.Green;
    }
}
