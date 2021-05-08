using Microsoft.Xna.Framework;
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

    private bool _isHighlighted = false;
    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {
            _isHighlighted = value;
            _highlight = (_isHighlighted ? 2f : 1f) * Vector3.One;
        }
    }

    private bool _isPreview = false;
    public bool IsPreview
    {
        get => _isPreview;
        set
        {
            _isPreview = value;
            _technique = _isPreview ? "PreviewEffect" : "LightEffect";

            if (_isPreview)
            {
                TDSceneManager.ActiveScene.MeshObjects.Remove(this);
                if (!TDSceneManager.ActiveScene.TransparentMeshObjects.Contains(this)) TDSceneManager.ActiveScene.TransparentMeshObjects.Add(this);
            }
            else
            {
                if (!TDSceneManager.ActiveScene.MeshObjects.Contains(this)) TDSceneManager.ActiveScene.MeshObjects.Add(this);
                TDSceneManager.ActiveScene.TransparentMeshObjects.Remove(this);
            }
        }
    }

    public Vector4 BaseColor = Vector4.One;

    private string _technique = "LightEffect";
    private Vector3 _highlight = Vector3.One;

    public override void Initialize()
    {
        base.Initialize();

        Effect = TDContentManager.LoadEffect("LightEffect");

        if (_isPreview && !TDSceneManager.ActiveScene.TransparentMeshObjects.Contains(this)) TDSceneManager.ActiveScene.TransparentMeshObjects.Add(this);
        else if (!IsPreview && !TDSceneManager.ActiveScene.MeshObjects.Contains(this)) TDSceneManager.ActiveScene.MeshObjects.Add(this);
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
        if (IsPreview) return;

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
                effect.CurrentTechnique = effect.Techniques[_technique];

                effect.Parameters["World"].SetValue(TDObject.Transform.TransformMatrix);
                effect.Parameters["ViewProjection"].SetValue(TDSceneManager.ActiveScene.CameraObject?.ViewProjectionMatrix ?? Matrix.Identity);

                effect.Parameters["Texture"].SetValue(Texture);

                effect.Parameters["LightPosition"].SetValue(TDSceneManager.ActiveScene.LightObject?.TDObject.Transform.Position ?? Vector3.Zero);
                effect.Parameters["LightViewProjection"].SetValue(TDSceneManager.ActiveScene.LightObject?.ViewProjectionMatrix ?? Matrix.Identity);

                effect.Parameters["AmbientIntensity"].SetValue(.2f);
                effect.Parameters["AmbientColor"].SetValue(_highlight);

                effect.Parameters["DiffuseIntensity"].SetValue(.8f);
                effect.Parameters["DiffuseColor"].SetValue(Vector3.One);

                effect.Parameters["BaseColor"].SetValue(BaseColor);

                effect.Parameters["Shadow"].SetValue(TDSceneManager.ActiveScene.ShadowRender);
                effect.Parameters["InvertedShadowSize"].SetValue(TDSceneManager.ActiveScene.InvertedShadowSize);
            }

            mesh.Draw();
        }
    }

    public void Highlight(bool highlight)
    {
        IsHighlighted = highlight;
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.MeshObjects.Remove(this);
        TDSceneManager.ActiveScene.TransparentMeshObjects.Remove(this);
    }
}
