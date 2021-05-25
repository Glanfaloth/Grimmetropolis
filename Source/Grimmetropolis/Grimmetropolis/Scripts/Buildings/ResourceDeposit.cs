using Microsoft.Xna.Framework;

using System.Diagnostics;

public enum ResourceDepositType
{
    Wood,
    Stone
}

public class ResourceDeposit : Structure
{
    public ResourceDepositType Type = ResourceDepositType.Wood;

    public int Storage = 10;
    public float RegenerationTime = 2f;
    public float HarvestTime = 1f;

    private float _time = 0f;

    private int _currentStorage = 10;
    public int CurrentStorage
    {
        get => _currentStorage;
        set
        {
            _currentStorage = value;
            if (_healthBar != null)
            {
                _healthBar.CurrentProgress = _currentStorage;
                _healthBar.Show();
            }
        }
    }

    public HealthBar _healthBar = null;

    public override void Initialize()
    {
        base.Initialize();

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 3f * Vector3.Backward;
        _healthBar = healthBarObject.GetComponent<HealthBar>();
        _healthBar.CurrentProgress = _currentStorage;
        _healthBar.MaxProgress = Storage;

        HarvestTime = Type switch
        {
            ResourceDepositType.Wood => Config.RESOURCE_WOOD_GATHER_DURATION,
            ResourceDepositType.Stone => Config.RESOURCE_STONE_GATHER_DURATION,
            _ => 1f
        };

        RegenerationTime = Type switch
        {
            ResourceDepositType.Wood => Config.RESOURCE_WOOD_REGENERATION_TIME,
            ResourceDepositType.Stone => Config.RESOURCE_STONE_REGENERATION_TIME,
            _ => 2f
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_currentStorage < Storage && _time >= RegenerationTime)
        {
            CurrentStorage++;
            _time = 0f;
        }
    }

    public void HarvestResource()
    {
        switch (Type)
        {
            case ResourceDepositType.Wood:
                GameManager.Instance.ResourcePool += new ResourcePile(Config.RESOURCE_WOOD_GATHER_BASE_RATE, 0);
                break;
            case ResourceDepositType.Stone:
                GameManager.Instance.ResourcePool += new ResourcePile(0, Config.RESOURCE_STONE_GATHER_BASE_RATE);
                break;
        }

        CurrentStorage--;
        if (CurrentStorage == Storage - 1) _time = 0f;
    }

    public override void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
        if (highlight) _healthBar.QuickShow();
        else _healthBar.QuickHide();
    }
}

