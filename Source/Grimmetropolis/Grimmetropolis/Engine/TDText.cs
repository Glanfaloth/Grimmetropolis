﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TDText : TDComponent
{
    public SpriteFont SpriteFont = TDContentManager.LoadSpriteFont("Montserrat");

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
    public float Depth = 0f;

    public float Width { get; private set; }
    public float Height { get; private set; }

    private bool _isShowing = true;
    public bool IsShowing
    {
        get => _isShowing;
        set
        {
            _isShowing = value;
            AddToList();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        AddToList();
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.TextObjects.Remove(this);
    }

    public void Draw()
    {
        TDSceneManager.SpriteBatch.DrawString(SpriteFont, Text, TDObject.RectTransform.Position, Color, TDObject.RectTransform.Rotation,
            TDObject.RectTransform.Origin, TDObject.RectTransform.Scale, SpriteEffects.None, Depth);
    }
    private void AddToList()
    {
        if (_isShowing && !TDSceneManager.ActiveScene.TextObjects.Contains(this)) TDSceneManager.ActiveScene.TextObjects.Add(this);
        else TDSceneManager.ActiveScene.TextObjects.Remove(this);
    }
}
