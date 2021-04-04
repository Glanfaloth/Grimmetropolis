using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

public class TDSprite : TDComponent
{
    public Texture2D Texture;
    public Color Color = Color.White;
    public float Depth = 0f;

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
            TDObject.RectTransform.Origin, TDObject.RectTransform.Scale, SpriteEffects.None, Depth);
    }
}
