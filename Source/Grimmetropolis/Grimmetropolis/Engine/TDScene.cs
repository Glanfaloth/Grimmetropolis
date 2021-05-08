using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using System.Collections.Generic;

public class TDScene
{
    public List<TDObject> TDObjects = new List<TDObject>();
    public List<TDObject> DeletedObjects = new List<TDObject>();
    public List<TDObject> CreatedObjects = new List<TDObject>();

    public TDCamera CameraObject;
    public TDLight LightObject;
    public List<TDMesh> MeshObjects = new List<TDMesh>();
    public List<TDMesh> TransparentMeshObjects = new List<TDMesh>();

    public List<TDCylinderCollider> CylinderColliderObjects = new List<TDCylinderCollider>();
    public TDCuboidCollider[,] CuboidColliderObjects = new TDCuboidCollider[0, 0];

    public RenderTarget2D ShadowRender;
    public Vector2 InvertedShadowSize;

    public List<TDRectTransform> UI3DObjects = new List<TDRectTransform>();
    public List<TDSprite> SpriteObjects = new List<TDSprite>();
    public List<TDText> TextObjects = new List<TDText>();

    public virtual void Initialize()
    {
        ShadowRender = new RenderTarget2D(TDSceneManager.Graphics.GraphicsDevice, 4096, 4096, true, SurfaceFormat.Single, DepthFormat.Depth24);
        InvertedShadowSize = new Vector2(1f / ShadowRender.Width, 1f / ShadowRender.Height);
    }

    public virtual void Update(GameTime gameTime)
    {
        // Initialize newly created TDObjects
        for (int i = 0; i < CreatedObjects.Count; i++)
        {
            CreatedObjects[i].Initialize();
        }
        if (CreatedObjects.Count > 0) CreatedObjects.Clear();

        // Delete marked TDObjects
        for (int i = 0; i < DeletedObjects.Count; i++)
        {
            DeletedObjects[i].DestroyAttachedObjects();
        }
        if (DeletedObjects.Count > 0) DeletedObjects.Clear();

        // Update TDActions
        for (int i = 0; i < TDObjects.Count; i++)
        {
            TDObjects[i].UpdateActions(gameTime);
        }

        // Update TDObjects
        for (int i = 0; i < TDObjects.Count; i++)
        {
            TDObjects[i].UpdateComponents(gameTime);
        }

        // Update cylinder collisions
        foreach(TDCylinderCollider cylinder in CylinderColliderObjects)
        {
            cylinder.UpdateColliderGeometry();
        }
        for (int i = 0; i < CylinderColliderObjects.Count; i++)
        {
            for (int j = i + 1; j < CylinderColliderObjects.Count; j++)
            {
                CylinderColliderObjects[i].UpdateCollision(CylinderColliderObjects[j]);
            }
        }
    }

    public void Draw()
    {
        TDSceneManager.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
        TDSceneManager.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        TDSceneManager.Graphics.GraphicsDevice.BlendState = BlendState.Opaque;

        // Update camera and light
        CameraObject?.UpdateCamera();
        LightObject?.UpdateLight();

        // Update UI3D objects
        foreach (TDRectTransform ui3DObject in UI3DObjects)
        {
            ui3DObject.UpdatePosition();
        }

        // Draw shadow render
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ShadowRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.DrawShadow();
        }
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(null);

        // Draw render
        for (int i = 0; i < MeshObjects.Count; i++)
        {
            MeshObjects[i].Draw();
        }
        for (int i = 0; i < TransparentMeshObjects.Count; i++)
        {
            TransparentMeshObjects[i].Draw();
        }

        // Draw sprites and strings
        TDSceneManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        foreach (TDSprite spriteObject in SpriteObjects)
        {
            spriteObject.Draw();
        }
        foreach (TDText textObject in TextObjects)
        {
            textObject.Draw();
        }
        TDSceneManager.SpriteBatch.End();
    }
}
