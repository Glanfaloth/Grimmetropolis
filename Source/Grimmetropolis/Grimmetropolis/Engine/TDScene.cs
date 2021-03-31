using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

public class TDScene
{
    public List<TDObject> TDObjects = new List<TDObject>();
    public List<TDObject> DeletedObjects = new List<TDObject>();
    public List<TDObject> CreatedObjects = new List<TDObject>();

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
        // Delete marked TDObjects
        for (int i = 0; i < DeletedObjects.Count; i++)
        {
            DeletedObjects[i].DestroyComponents();
        }
        if (DeletedObjects.Count > 0) DeletedObjects.Clear();

        // Initialize newly created TDObjects
        for (int i = 0; i < CreatedObjects.Count; i++)
        {
            CreatedObjects[i].Initialize();
        }
        if (CreatedObjects.Count > 0) CreatedObjects.Clear();

        // Update TDObjects
        for (int i = 0; i < TDObjects.Count; i++)
        {
            TDObjects[i].Update(gameTime);
        }

        // Update collisions
        foreach (TDCollider colliderObject in ColliderObjects)
        {
            colliderObject.UpdateColliderGeometry();
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
        // Update camera and light
        CameraObject?.UpdateCamera();
        LightObject?.UpdateLight();

        // Draw shadow render
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ShadowRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.DrawShadow();
        }

        // Draw final render
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ImageRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.Draw();
        }

        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(null);
    }
}
