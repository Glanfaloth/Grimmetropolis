﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Linq;

public class TDMesh : TDComponent
{
    private Model _model;
    public Model Model
    {
        get => _model;
        set
        {
            _model = value;
            if (_effect != null) SetModelEffect();
        }
    }

    public Texture2D Texture;

    private Effect _effect;

    public Effect Effect
    {
        get => _effect;
        set
        {
            _effect = value;
            if (_model != null) SetModelEffect();
        }
    }

    public bool IsBlueprint { get; internal set; }

    public float BaseHighlightFactor = 1f;
    private float _highlightFactor = 1f;

    public override void Initialize()
    {
        base.Initialize();

        Effect = TDContentManager.LoadEffect("LightEffect");

        TDSceneManager.ActiveScene.MeshObjects.Add(this);
    }

    private void SetModelEffect()
    {
        foreach (ModelMesh mesh in _model?.Meshes ?? Enumerable.Empty<ModelMesh>())
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect.Clone();
            }
        }
    }

    public void DrawShadow()
    {
        foreach (ModelMesh mesh in _model?.Meshes ?? Enumerable.Empty<ModelMesh>())
        {
            foreach (Effect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques["ShadowEffect"];

                effect.Parameters["World"].SetValue(TDObject.Transform.TransformMatrix);
                effect.Parameters["LightViewProjection"].SetValue(TDSceneManager.ActiveScene.LightObject?.ViewProjectionMatrix ?? Matrix.Identity);
            }

            mesh.Draw();
        }
    }

    public void Draw()
    {
        foreach (ModelMesh mesh in _model?.Meshes ?? Enumerable.Empty<ModelMesh>())
        {
            foreach (Effect effect in mesh.Effects)
            {
                effect.CurrentTechnique = IsBlueprint 
                    ? effect.Techniques["BlueprintEffect"]
                    : effect.Techniques["LightEffect"];

                effect.Parameters["World"].SetValue(TDObject.Transform.TransformMatrix);
                effect.Parameters["ViewProjection"].SetValue(TDSceneManager.ActiveScene.CameraObject?.ViewProjectionMatrix ?? Matrix.Identity);

                effect.Parameters["Texture"].SetValue(Texture);

                effect.Parameters["LightPosition"].SetValue(TDSceneManager.ActiveScene.LightObject?.TDObject.Transform.Position ?? Vector3.Zero);
                effect.Parameters["LightViewProjection"].SetValue(TDSceneManager.ActiveScene.LightObject?.ViewProjectionMatrix ?? Matrix.Identity);

                effect.Parameters["AmbientIntensity"].SetValue(.2f);
                effect.Parameters["AmbientColor"].SetValue(_highlightFactor * Vector3.One);

                effect.Parameters["DiffuseIntensity"].SetValue(.8f);
                effect.Parameters["DiffuseColor"].SetValue(Vector3.One);

                effect.Parameters["Shadow"].SetValue(TDSceneManager.ActiveScene.ShadowRender);
                effect.Parameters["InvertedShadowSize"].SetValue(TDSceneManager.ActiveScene.InvertedShadowSize);
            }

            mesh.Draw();
        }
    }

    public void Highlight(bool highlight)
    {
        _highlightFactor = highlight ? 2f : BaseHighlightFactor;
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.MeshObjects.Remove(this);
    }
}
