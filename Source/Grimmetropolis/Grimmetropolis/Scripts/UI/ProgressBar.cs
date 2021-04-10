using Microsoft.Xna.Framework;

public class ProgressBar : TDComponent
{
    public TDSprite Background;
    public TDSprite Foreground;

    public float CurrentProgress;
    public float MaxProgress;

    protected bool _isShowing = true;
    private bool _requiresHide = false;

    public override void Initialize()
    {
        base.Initialize();

        SetProgressBar();
        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_requiresHide) Hide();
    }

    public void Show()
    {
        SetProgressBar();

        if (_isShowing) return;

        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Background)) TDSceneManager.ActiveScene.SpriteObjects.Add(Background);
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Foreground)) TDSceneManager.ActiveScene.SpriteObjects.Add(Foreground);

        _isShowing = true;
    }

    public void Hide()
    {
        if (!_isShowing) return;

        if (TDSceneManager.ActiveScene.SpriteObjects.Contains(Background) && TDSceneManager.ActiveScene.SpriteObjects.Contains(Foreground))
        {
            TDSceneManager.ActiveScene.SpriteObjects.Remove(Background);
            TDSceneManager.ActiveScene.SpriteObjects.Remove(Foreground);

            _isShowing = false;
            _requiresHide = false;
        }
        else _requiresHide = true;
    }

    public virtual void SetProgressBar()
    {
        Foreground.TDObject.RectTransform.Scale = new Vector2(CurrentProgress / MaxProgress, 1f);
    }
}
