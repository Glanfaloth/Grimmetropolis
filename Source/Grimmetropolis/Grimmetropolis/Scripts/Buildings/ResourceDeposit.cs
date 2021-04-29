﻿using Microsoft.Xna.Framework;

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
            if (HealthBar != null)
            {
                HealthBar.CurrentProgress = _currentStorage;
                HealthBar.Show();
            }
        }
    }

    public HealthBar HealthBar = null;

    public override void Initialize()
    {
        base.Initialize();

        TDObject healthBarObject = PrefabFactory.CreatePrefab(PrefabType.HealthBar, TDObject.Transform);
        healthBarObject.RectTransform.Offset = 2.5f * Vector3.Backward;
        HealthBar = healthBarObject.GetComponent<HealthBar>();
        HealthBar.CurrentProgress = _currentStorage;
        HealthBar.MaxProgress = Storage;

        HarvestTime = Type switch
        {
            ResourceDepositType.Wood => Config.RESOURCE_WOOD_GATHER_DURATION,
            ResourceDepositType.Stone => Config.RESOURCE_STONE_GATHER_DURATION,
            _ => 1f
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
        if (_currentStorage <= 0) return;

        switch (Type)
        {
            case ResourceDepositType.Wood:
                GameManager.Instance.ResourcePool += new ResourcePile(Config.RESOURCE_WOOD_GATHER_BASE_RATE, 0);
                Debug.WriteLine("Wood collected to " + GameManager.Instance.ResourcePool.Wood);
                break;
            case ResourceDepositType.Stone:
                GameManager.Instance.ResourcePool += new ResourcePile(0, Config.RESOURCE_STONE_GATHER_BASE_RATE);
                Debug.WriteLine("Stone collected to " + GameManager.Instance.ResourcePool.Stone);
                break;
        }

        CurrentStorage--;
    }
}

