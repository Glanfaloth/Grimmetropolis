using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TDSprite : TDUI
{
    public Texture2D Texture;
    public Color Color = Color.White;

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.SpriteObjects.Remove(this);
    }

    public void Draw()
    {
        Vector2 position = Vector2.Clamp(TDObject.RectTransform.Position, TDObject.RectTransform.MinPosition, TDObject.RectTransform.MaxPosition);
        TDSceneManager.SpriteBatch.Draw(Texture, position, null, Color,TDObject.RectTransform.Rotation,
            TDObject.RectTransform.Origin, TDObject.RectTransform.Scale, SpriteEffects.None, Depth);
    }

    protected override void AddToList()
    {
        if (IsShowing)
        {
            if (!TDSceneManager.ActiveScene.SpriteObjects.Contains(this)) TDSceneManager.ActiveScene.SpriteObjects.Add(this);
        }
        else TDSceneManager.ActiveScene.SpriteObjects.Remove(this);
    }
}
