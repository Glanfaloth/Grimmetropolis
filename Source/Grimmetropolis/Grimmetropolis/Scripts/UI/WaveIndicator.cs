using Microsoft.Xna.Framework;

using System;

public class WaveIndicator : TDComponent
{
    public TDSprite Image;
    public TDText Info;

    public override void Initialize()
    {
        base.Initialize();

        Vector2 startPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 10f);
        Vector2 endPosition = new Vector2(TDSceneManager.Graphics.PreferredBackBufferWidth - 10f, 40f);

        TDObject.RunAction(1f, (p) =>
        {
            TDObject.RectTransform.Position = Vector2.Lerp(startPosition, endPosition, .5f + .5f * MathF.Sin(MathHelper.TwoPi * p));
        }, true);

        TDObject.RectTransform.Scale = 2f * Vector2.One;
    }
}
