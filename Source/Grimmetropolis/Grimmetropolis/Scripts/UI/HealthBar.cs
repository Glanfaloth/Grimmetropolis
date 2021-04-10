using Microsoft.Xna.Framework;

using System.Diagnostics;

public class HealthBar : TDComponent
{
    public TDSprite Background;
    public TDSprite Foreground;

    public float Health = 1f;
    public float BaseHealth = 1f;

    private bool _isShowing = true;
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
        Show();
        _time = _revealTime;

        Health = health;
        float healthPercentage = Health / BaseHealth;

        if (healthPercentage < .25f) Foreground.Color = Color.Red;
        else if (healthPercentage < .5f) Foreground.Color = Color.Yellow;
        else Foreground.Color = Color.Green;

        Foreground.TDObject.RectTransform.Scale = new Vector2(healthPercentage, 1f);
    }

    public void Show()
    {
        if (_isShowing) return;
        _isShowing = true;

        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Background)) TDSceneManager.ActiveScene.SpriteObjects.Add(Background);
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Foreground)) TDSceneManager.ActiveScene.SpriteObjects.Add(Foreground);
    }

    public void Hide()
    {
        if (!_isShowing) return;
        _isShowing = false;

        TDSceneManager.ActiveScene.SpriteObjects.Remove(Background);
        TDSceneManager.ActiveScene.SpriteObjects.Remove(Foreground);
    }
}
