using Microsoft.Xna.Framework.Graphics;

public class TDMesh : TDComponent
{
    private Model _model;
    private Texture2D _texture;

    public TDMesh(TDObject tdObject, Model model, Texture2D texture) : base(tdObject)
    {
        _model = model;
        _texture = texture;

        TDSceneManager.ActiveScene.MeshObjects.Add(this);
    }

    public void Draw()
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.TextureEnabled = true;
                effect.Texture = _texture;

                effect.World = TDObject.Transform.TransformMatrix;
                effect.View = TDSceneManager.ActiveScene.CameraObject.ViewMatrix;
                effect.Projection = TDSceneManager.ActiveScene.CameraObject.ProjectionMatrix;

                effect.EnableDefaultLighting();
            }

            mesh.Draw();
        }
    }
}
