using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

public abstract class Building : Structure, ITarget
{
    public virtual ResourcePile GetResourceCost() => new ResourcePile(1, 1);
    public virtual ResourcePile GetResourceUpkeep() => new ResourcePile();
    public virtual MapTileType GetRequiredMapTileType() => MapTileType.Ground;

    public override bool CanBeAttacked => true;

    public float BaseHealth = Config.BUILDING_DEFAULT_HEALTH;

    protected float _health = Config.BUILDING_DEFAULT_HEALTH;

    protected bool _missingUpkeep = false;
    public virtual bool MissingUpkeep
    {
        get => _missingUpkeep;
        set => _missingUpkeep = value;
    }

    public virtual float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_healthBar != null)
            {
                _healthBar.CurrentProgress = _health;
                _healthBar.Show();
            }
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    public virtual float BuildTime { get; } = Config.OUTPOST_BUILD_VALUE;

    public Vector3 OffsetTarget { get; } = .5f * Vector3.Backward;

    TDObject ITarget.TDObject => TDObject;

    public bool IsBlueprint;

    private HealthBar _healthBar;
    protected float _buildProgress;
    private ProgressBar _progressBar;

    private float _durationResourceUpkeep = 1f;
    private float _timeResourceUpkeep = 1f;

    public override void Initialize()
    {
        if (Mesh.IsPreview) _health = 0f;

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 4f * Vector3.Backward;
        _healthBar = healthBarObject.GetComponent<HealthBar>();
        _healthBar.CurrentProgress = Health;
        _healthBar.MaxProgress = BaseHealth;

        if (IsPreview) SetAsPreview();
        if (IsBlueprint) SetAsBlueprint();

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!IsPreview)
        {
            if (!IsBlueprint)
            {
                _timeResourceUpkeep -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeResourceUpkeep <= 0f)
                {
                    MissingUpkeep = !ResourcePile.CheckAvailability(GameManager.Instance.ResourcePool, GetResourceUpkeep());
                    _timeResourceUpkeep += _durationResourceUpkeep;
                    GameManager.Instance.ResourcePool -= GetResourceUpkeep();
                    GameManager.Instance.ResourcePool = ResourcePile.Max(GameManager.Instance.ResourcePool, new ResourcePile(0, 0, 0));
                }
            }
            _progressBar?.Show();
        }
    }

    public override void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
        if (highlight) _healthBar.QuickShow();
        else _healthBar.QuickHide();
    }

    protected virtual void SetAsPreview()
    {
        IsPreview = true;
        IsBlueprint = true;
        Mesh.IsPreview = true;
        Mesh.BaseColor = new Vector4(.1f, .1f, .1f, .5f);
    }

    protected virtual void SetAsBlueprint()
    {
        _buildProgress = 0;
        Mesh.IsPreview = true;
        Mesh.BaseColor = new Vector4(.1f, .1f, .1f, .5f);

        TDObject progessBarObject = PrefabFactory.CreatePrefab(PrefabType.ProgressBar, TDObject.Transform);
        progessBarObject.RectTransform.Offset = 3f * Vector3.Backward;
        _progressBar = progessBarObject.GetComponent<ProgressBar>();
        _progressBar.CurrentProgress = _buildProgress;
        _progressBar.MaxProgress = BuildTime;
        _progressBar.Show();
    }

    public virtual bool TryBuild(float buildStrength)
    {
        if(IsBlueprint)
        {
            _buildProgress += buildStrength;
            _progressBar.CurrentProgress = _buildProgress;
            if (_buildProgress >= BuildTime)
            {
                IsBlueprint = false;
                _progressBar.Hide();
                _progressBar.TDObject.Destroy();
                _progressBar = null;
                Mesh.IsPreview = false;
                Mesh.BaseColor = Vector4.One;

                Health = BaseHealth;
            }
            return true;
        }

        return false;
    }

    public bool TryRepair(float buildStrength)
    {
        if (Health >= BaseHealth) return false;
        Health = MathHelper.Min(Health + buildStrength, BaseHealth);
        return true;
    }

    public bool CheckPlacability(MapTile mapTile)
    {
        int xHigh = mapTile.Position.X + Size.X;
        int yHigh = mapTile.Position.Y + Size.Y;

        if (xHigh >= GameManager.Instance.Map.Width) return false;
        if (yHigh >= GameManager.Instance.Map.Height) return false;

        for (int x = mapTile.Position.X; x < xHigh; x++)
        {
            for (int y = mapTile.Position.Y; y < yHigh; y++)
            {
                if (GameManager.Instance.Map.MapTiles[x, y].Type != GetRequiredMapTileType()
                    || GameManager.Instance.Map.MapTiles[x, y].Item != null
                    || GameManager.Instance.Map.MapTiles[x, y].Structure != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected void CreateBuildingPart(Model model, Texture2D texture, string buildingPart, out TDTransform buildingPartTransform, out TDMesh buildingPartMesh)
    {
        TDObject buildingPartObject = PrefabFactory.CreatePrefab(PrefabType.Empty, TDObject.Transform);
        buildingPartMesh = buildingPartObject.AddComponent<TDMesh>();

        ModelBone bone; model.Bones.TryGetValue(buildingPart, out bone);
        ModelMesh mesh; model.Meshes.TryGetValue(buildingPart, out mesh);

        buildingPartMesh.Model = new Model(TDSceneManager.Graphics.GraphicsDevice, new List<ModelBone>() { bone }, new List<ModelMesh>() { mesh });
        buildingPartMesh.Texture = texture;

        buildingPartTransform = buildingPartObject.Transform;
    }
    protected void CreateMainBuildingPart(Model model, Texture2D texture, string buildingPart)
    {
        ModelBone bone; model.Bones.TryGetValue(buildingPart, out bone);
        ModelMesh mesh; model.Meshes.TryGetValue(buildingPart, out mesh);

        Mesh.Model = new Model(TDSceneManager.Graphics.GraphicsDevice, new List<ModelBone>() { bone }, new List<ModelMesh>() { mesh });
        Mesh.Texture = texture;
    }
}
