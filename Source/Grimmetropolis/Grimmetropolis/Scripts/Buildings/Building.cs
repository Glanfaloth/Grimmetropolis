﻿using Microsoft.Xna.Framework;

public abstract class Building : Structure, ITDTarget
{
    public abstract ResourcePile GetResourceCost();

    public override bool CanBeAttacked => true;

    public float BaseHealth = Config.BUILDING_DEFAULT_HEALTH;

    private float _health = Config.BUILDING_DEFAULT_HEALTH;
    public float Health
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

    public Vector3 OffsetTarget { get; } = .5f * Vector3.Backward;

    TDObject ITDTarget.TDObject => TDObject;

    private HealthBar _healthBar;

    public override void Initialize()
    {
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
}
