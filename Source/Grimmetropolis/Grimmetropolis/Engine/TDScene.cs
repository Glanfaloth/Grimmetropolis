using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

    public Texture2D Background;
    public RenderTarget2D ShadowRender;
    public Vector2 InvertedShadowSize;

    public List<TDRectTransform> UI3DObjects = new List<TDRectTransform>();
    public List<TDSprite> SpriteObjects = new List<TDSprite>();
    public List<TDText> TextObjects = new List<TDText>();

    public List<TDSound> SoundObjects = new List<TDSound>();

    public bool RequiresLoadingScene = false;
    public bool F1KeyState = false;
    public bool ShowUI = true;

    public virtual void Initialize()
    {
        Background = TDContentManager.LoadTexture("Background");
        ShadowRender = new RenderTarget2D(TDSceneManager.Graphics.GraphicsDevice, 4096, 4096, true, SurfaceFormat.Single, DepthFormat.Depth24);
        InvertedShadowSize = new Vector2(1f / ShadowRender.Width, 1f / ShadowRender.Height);
    }

    public virtual void Update(GameTime gameTime)
    {
        // Check debug options
        KeyboardState keys = Keyboard.GetState();
        if (keys.IsKeyDown(Keys.F1)) F1KeyState = true;
        if (F1KeyState && keys.IsKeyUp(Keys.F1))
        {
            F1KeyState = false;
            ShowUI = !ShowUI;
        }
        if (keys.IsKeyDown(Keys.F2))
        {
            TDSound.Volume = MathHelper.Max(TDSound.Volume - .4f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
            foreach (TDSound soundObject in SoundObjects)
            {
                soundObject.SetVolume();
            }
        }
        if (keys.IsKeyDown(Keys.F3))
        {
            TDSound.Volume = MathHelper.Min(TDSound.Volume + .4f * (float)gameTime.ElapsedGameTime.TotalSeconds, 1f);
            foreach (TDSound soundObject in SoundObjects)
            {
                soundObject.SetVolume();
            }
        }

        // Load scene if necessary
        if (RequiresLoadingScene)
        {
            foreach(TDSound soundObject in SoundObjects)
            {
                soundObject.Stop();
            }
            TDSceneManager.LoadTemporaryScene();
            return;
        }

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

        // Update camera and light
        CameraObject?.UpdateCamera();
        LightObject?.UpdateLight();

        // Update UI3D objects
        foreach (TDRectTransform ui3DObject in UI3DObjects)
        {
            ui3DObject.UpdatePosition();
        }

        TDSceneManager.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        TDSceneManager.Graphics.GraphicsDevice.BlendState = BlendState.Opaque;

        // Draw shadow render
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(ShadowRender);
        foreach (TDMesh meshObject in MeshObjects)
        {
            meshObject.DrawShadow();
        }
        TDSceneManager.Graphics.GraphicsDevice.SetRenderTarget(null);

        TDSceneManager.SpriteBatch.Begin();
        TDSceneManager.SpriteBatch.Draw(Background, Vector2.Zero, Color.White);
        TDSceneManager.SpriteBatch.End();

        TDSceneManager.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        TDSceneManager.Graphics.GraphicsDevice.BlendState = BlendState.Opaque;

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
        if (ShowUI)
        {
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
}