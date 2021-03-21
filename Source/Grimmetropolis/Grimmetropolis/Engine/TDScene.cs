using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDScene
{

    public List<TDObject> TDObjects = new List<TDObject>();

    public TDCamera CameraObject;
    public List<TDMesh> MeshObjects = new List<TDMesh>();
    public List<TDCollider> ColliderObjects = new List<TDCollider>();

    public virtual void Initialize() { }

    public void Update(GameTime gameTime)
    {
        foreach (TDObject tdObject in TDObjects)
        {
            tdObject.Update(gameTime);
        }

        for (int i = 0; i < ColliderObjects.Count; i++)
        {
            for (int j = i + 1; j < ColliderObjects.Count; j++)
            {
                ColliderObjects[i].UpdateCollision(ColliderObjects[j]);
            }
        }
    }

    public void Draw()
    {
        CameraObject?.UpdateCamera();

        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.Draw();
        }
    }
}
