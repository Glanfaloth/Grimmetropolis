using Microsoft.Xna.Framework;

using System;

public class WaveIndicator : TDComponent
{
    public TDSprite Image;
    public TDText Info;

    protected bool _isShowing = false;

    public override void Initialize()
    {
        base.Initialize();

        Vector2 startPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 10f);
        Vector2 endPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 40f);

        TDObject.RunAction(1f, (p) =>
        {
            TDObject.RectTransform.Position = Vector2.Lerp(startPosition, endPosition, .5f + .5f * MathF.Sin(MathHelper.TwoPi * p));
        }, true);

        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _isShowing = GameManager.Instance.EnemyController._waveIndicator;

        if (_isShowing) Show();
        else if (!_isShowing) Hide();
    }

    public virtual void Show()
    {
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Image)) TDSceneManager.ActiveScene.SpriteObjects.Add(Image);
        if (!TDSceneManager.ActiveScene.TextObjects.Contains(Info)) TDSceneManager.ActiveScene.TextObjects.Add(Info);
    }

    public virtual void Hide()
    {
        if (TDSceneManager.ActiveScene.SpriteObjects.Contains(Image)) TDSceneManager.ActiveScene.SpriteObjects.Remove(Image); 
        if (TDSceneManager.ActiveScene.TextObjects.Contains(Info)) TDSceneManager.ActiveScene.TextObjects.Remove(Info);
    }
}
