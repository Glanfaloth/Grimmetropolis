using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDScene
{

    public List<TDObject> TDObjects = new List<TDObject>();

    public TDCameraComponent CameraObject;
    public List<TDMeshComponent> MeshObjects = new List<TDMeshComponent>();

    public virtual void Initialize() { }

    public void Update(GameTime gameTime)
    {
        foreach (TDObject tdObject in TDObjects)
        {
            tdObject.Update(gameTime);
        }
    }

    public void Draw()
    {
        foreach (TDMeshComponent meshObject in MeshObjects)
        {
            meshObject.Draw();
        }
    }
}
