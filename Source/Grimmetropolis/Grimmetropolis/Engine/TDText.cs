using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TDText : TDUI
{
    public SpriteFont SpriteFont = TDContentManager.LoadSpriteFont("Righteous");

    private string _text;
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Vector2 size = SpriteFont.MeasureString(_text);
            Width = size.X;
            Height = size.Y;
        }
    }

    public Color Color = Color.White;

    public float Width { get; private set; }
    public float Height { get; private set; }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.TextObjects.Remove(this);
    }

    public void Draw()
    {
        Vector2 position = Vector2.Clamp(TDObject.RectTransform.Position, TDObject.RectTransform.MinPosition, TDObject.RectTransform.MaxPosition);
        TDSceneManager.SpriteBatch.DrawString(SpriteFont, Text, position, Color, TDObject.RectTransform.Rotation,
            TDObject.RectTransform.Origin, TDObject.RectTransform.Scale, SpriteEffects.None, Depth);
    }
    protected override void AddToList()
    {
        if (IsShowing)
        {
            if (!TDSceneManager.ActiveScene.TextObjects.Contains(this)) TDSceneManager.ActiveScene.TextObjects.Add(this);
        }
        else TDSceneManager.ActiveScene.TextObjects.Remove(this);
    }
}
