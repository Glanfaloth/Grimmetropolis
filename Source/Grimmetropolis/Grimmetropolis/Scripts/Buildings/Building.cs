using Microsoft.Xna.Framework;
using System;

public abstract class Building : Structure, ITarget
{
    public abstract ResourcePile GetResourceCost();

    public override bool CanBeAttacked => true;

    public float BaseHealth = Config.BUILDING_DEFAULT_HEALTH;

    protected float _health = Config.BUILDING_DEFAULT_HEALTH;
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

    public abstract float BuildTime { get; }

    public Vector3 OffsetTarget { get; } = .5f * Vector3.Backward;

    TDObject ITarget.TDObject => TDObject;

    private HealthBar _healthBar;
    private bool _isBlueprint;
    private float _buildProgress;
    private ProgressBar _progressBar;

    protected abstract void DoUpdate(GameTime gameTime);

    public sealed override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!_isBlueprint)
        {
            DoUpdate(gameTime);
        }
        else
        {
            _progressBar.Show();
        }
    }

    public override void Initialize()
    {
        if (Mesh.IsBlueprint) _health = 0f;

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 4f * Vector3.Backward;
        _healthBar = healthBarObject.GetComponent<HealthBar>();
        _healthBar.CurrentProgress = Health;
        _healthBar.MaxProgress = BaseHealth;


        base.Initialize();
    }
    public override void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
        if (highlight) _healthBar.QuickShow();
        else _healthBar.QuickHide();
    }

    internal void SetAsBlueprint()
    {
        _isBlueprint = true;
        _buildProgress = 0;

        TDObject progessBarObject = PrefabFactory.CreatePrefab(PrefabType.ProgressBar, TDObject.Transform);
        progessBarObject.RectTransform.Offset = 3f * Vector3.Backward;
        _progressBar = progessBarObject.GetComponent<ProgressBar>();
        _progressBar.CurrentProgress = _buildProgress;
        _progressBar.MaxProgress = BuildTime;
        _progressBar.Show();
        Mesh.IsBlueprint = true;
    }

    internal bool TryBuild(float buildStrength)
    {
        if(_isBlueprint)
        {
            _buildProgress += buildStrength;
            _progressBar.CurrentProgress = _buildProgress;
            if (_buildProgress >= BuildTime)
            {
                _isBlueprint = false;
                _progressBar.Hide();
                _progressBar.Destroy();
                _progressBar = null;
                Mesh.IsBlueprint = false;

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
}
