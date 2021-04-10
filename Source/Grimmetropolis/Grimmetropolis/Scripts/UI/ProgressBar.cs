using Microsoft.Xna.Framework;

public class ProgressBar : TDComponent
{
    public TDSprite Background;
    public TDSprite Foreground;

    public float CurrentProgress;
    public float MaxProgress;

    private bool _isShowing = true;

    public void Show()
    {
        SetProgressBar();

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

    public void SetProgressBar()
    {
        Foreground.TDObject.RectTransform.Scale = new Vector2(CurrentProgress / MaxProgress, 1f);
    }
}
