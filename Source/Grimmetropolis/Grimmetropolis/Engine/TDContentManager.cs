using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

public static class TDContentManager
{
    public static ContentManager Content { private get; set; }

    private static Dictionary<string, Model> _loadedModels = new Dictionary<string, Model>();
    private static Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, Effect> _loadedEffects = new Dictionary<string, Effect>();
    private static Dictionary<string, SpriteFont> _loadedSpriteFonts = new Dictionary<string, SpriteFont>();

    public static Model LoadModel(string name)
    {
            if (!_loadedModels.ContainsKey(name))
        {
            Model model = Content.Load<Model>(@"Models\" + name);
            _loadedModels.Add(name, model);

            return model;
        }
        
        return _loadedModels[name];
    }

    public static Texture2D LoadTexture(string name)
    {
        if (!_loadedTextures.ContainsKey(name))
        {
            Texture2D texture = Content.Load<Texture2D>(@"Textures\" + name);
            _loadedTextures.Add(name, texture);

            return texture;
        }

        return _loadedTextures[name];
    }

    public static Effect LoadEffect(string name)
    {
        if (!_loadedEffects.ContainsKey(name))
        {
            Effect effect = Content.Load<Effect>(@"Effects\" + name);
            _loadedEffects.Add(name, effect);

            return effect;
        }

        return _loadedEffects[name];
    }

    public static SpriteFont LoadSpriteFont(string name)
    {
        if (!_loadedSpriteFonts.ContainsKey(name))
        {
            SpriteFont effect = Content.Load<SpriteFont>(@"Fonts\" + name);
            _loadedSpriteFonts.Add(name, effect);

            return effect;
        }

        return _loadedSpriteFonts[name];
    }
}
