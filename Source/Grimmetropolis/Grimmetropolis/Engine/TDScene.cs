using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

public class TDScene
{
    public List<TDObject> TDObjects = new List<TDObject>();
    // Used to ensure some objects are updated before others
    public List<TDObject> UpdateFirstObjects = new List<TDObject>();

    public TDCamera CameraObject;
    public TDLight LightObject;
    public List<TDMesh> MeshObjects = new List<TDMesh>();
    public List<TDCollider> ColliderObjects = new List<TDCollider>();

    public RenderTarget2D ShadowRender;
    public RenderTarget2D ImageRender;

    public virtual void Initialize()
    {
        ShadowRender = new RenderTarget2D(TDSceneManager.Graphics.GraphicsDevice, 2048, 2048, true, SurfaceFormat.Single, DepthFormat.Depth24);
        ImageRender = new RenderTarget2D(TDSceneManager.Graphics.GraphicsDevice, 2 * TDSceneManager.Graphics.GraphicsDevice.Viewport.Width,
            2 * TDSceneManager.Graphics.GraphicsDevice.Viewport.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);
    }

    public void Update(GameTime gameTime)
    {
        foreach (TDObject tdObject in UpdateFirstObjects)
        {
            tdObject.Update(gameTime);
        }

        foreach (TDObject tdObject in TDObjects)
        {
            tdObject.Update(gameTime);
        }

        foreach (TDCollider colliderObject in ColliderObjects)
        {
            colliderObject.UpdateCollision();
        }
        for (int i = 0; i < ColliderObjects.Count; i++)
        {
            for (int j = i + 1; j < ColliderObjects.Count; j++)
            {
                ColliderObjects[i].Collide(ColliderObjects[j]);
            }
        }
    }

    public void Draw()
    {
        CameraObject?.UpdateCamera();
        LightObject?.UpdateLight();

        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ShadowRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.DrawShadow();
        }

        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ImageRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.DrawModel();
        }

        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(null);
    }
}
