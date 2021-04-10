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
    public float HarvestTime = 1f;

    public override void Initialize()
    {
        base.Initialize();

        HarvestTime = Type switch
        {
            ResourceDepositType.Wood => Config.RESOURCE_WOOD_GATHER_DURATION,
            ResourceDepositType.Stone => Config.RESOURCE_STONE_GATHER_DURATION,
            _ => 1f
        };
    }

    public void HarvestResource()
    {
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
    }
}

