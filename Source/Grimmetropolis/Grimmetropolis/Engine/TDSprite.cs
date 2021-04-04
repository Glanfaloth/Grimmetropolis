using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TDSprite : TDComponent
{
    public Texture2D Texture;

    public Color Color;

    public override void Initialize()
    {
        base.Initialize();

        TDSceneManager.ActiveScene.SpriteObjects.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.SpriteObjects.Remove(this);
    }

    public void Draw()
    {
        TDSceneManager.SpriteBatch.Draw(Texture, TDObject.RectTransform.Position, null, Color,TDObject.RectTransform.Rotation,
            TDObject.RectTransform.Origin, TDObject.RectTransform.Scale, SpriteEffects.None, 0f);
    }
}
