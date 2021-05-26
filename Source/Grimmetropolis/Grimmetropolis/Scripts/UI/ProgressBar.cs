using Microsoft.Xna.Framework;

public class ProgressBar : TDComponent
{
    public TDSprite Background;
    public TDSprite Foreground;

    public float CurrentProgress = 0f;
    public float MaxProgress = 1f;

    protected bool _isShowing = true;
    private bool _requiresHide = false;

    protected float MaxWidth = 0f;

    public override void Initialize()
    {
        base.Initialize();

        MaxWidth = Foreground.TDObject.RectTransform.Scale.X;

        SetProgressBar();
        Hide();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_requiresHide) Hide();
    }

    public virtual void Show()
    {
        SetProgressBar();

        if (_isShowing) return;

        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Background)) TDSceneManager.ActiveScene.SpriteObjects.Add(Background);
        if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(Foreground)) TDSceneManager.ActiveScene.SpriteObjects.Add(Foreground);

        _isShowing = true;
    }

    public virtual void Hide()
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

    protected virtual void SetProgressBar()
    {
        Vector2 currentScale = Foreground.TDObject.RectTransform.Scale;
        currentScale.X = CurrentProgress / MaxProgress * MaxWidth;
        Foreground.TDObject.RectTransform.Scale = currentScale;
    }
}
