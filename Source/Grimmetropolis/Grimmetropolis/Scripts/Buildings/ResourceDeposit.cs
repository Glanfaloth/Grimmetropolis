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

    public void HarvestResource()
    {
        switch (Type)
        {
            case ResourceDepositType.Wood:
                GameManager.Instance.ResourcePool += new ResourcePile(1f, 0f);
                Debug.WriteLine("Wood collected to " + GameManager.Instance.ResourcePool.Wood);
                break;
            case ResourceDepositType.Stone:
                GameManager.Instance.ResourcePool += new ResourcePile(0f, 1f);
                Debug.WriteLine("Stone collected to " + GameManager.Instance.ResourcePool.Stone);
                break;
        }
    }
}

