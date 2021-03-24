using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TDMesh : TDComponent
{
    private Model _model;
    private Texture2D _texture;
    private Effect _effect;

    public TDMesh(TDObject tdObject, string model, string texture) : base(tdObject)
    {
        _model = TDContentManager.LoadModel(model);
        _texture = TDContentManager.LoadTexture(texture);
        _effect = TDContentManager.LoadEffect("LightEffect");

        ApplyEffect();

        TDSceneManager.ActiveScene.MeshObjects.Add(this);
    }

    private void ApplyEffect()
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                meshPart.Effect = _effect.Clone();
            }
        }
    }

    public void DrawShadow()
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (Effect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques["ShadowEffect"];

                effect.Parameters["World"].SetValue(TDObject.Transform.TransformMatrix);
                effect.Parameters["LightViewProjection"].SetValue(TDSceneManager.ActiveScene.LightObject.ViewProjectionMatrix);
            }

            mesh.Draw();
        }
    }

    public void Draw()
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (Effect effect in mesh.Effects)
            {
                effect.CurrentTechnique = effect.Techniques["LightEffect"];

                effect.Parameters["World"].SetValue(TDObject.Transform.TransformMatrix);
                effect.Parameters["ViewProjection"].SetValue(TDSceneManager.ActiveScene.CameraObject.ViewProjectionMatrix);

                effect.Parameters["Texture"].SetValue(_texture);

                effect.Parameters["LightPosition"].SetValue(TDSceneManager.ActiveScene.LightObject.TDObject.Transform.Position);
                effect.Parameters["LightViewProjection"].SetValue(TDSceneManager.ActiveScene.LightObject.ViewProjectionMatrix);

                effect.Parameters["AmbientIntensity"].SetValue(.2f);
                effect.Parameters["AmbientColor"].SetValue(Vector3.One);

                effect.Parameters["DiffuseIntensity"].SetValue(.8f);
                effect.Parameters["DiffuseColor"].SetValue(Vector3.One);

                effect.Parameters["Shadow"].SetValue(TDSceneManager.ActiveScene.ShadowRender);
            }

            mesh.Draw();
        }
    }
}
