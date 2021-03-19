using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDScene
{

    public List<TDObject> TDObjects = new List<TDObject>();

    public TDCamera CameraObject;
    public List<TDMesh> MeshObjects = new List<TDMesh>();

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
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.Draw();
        }
    }
}
