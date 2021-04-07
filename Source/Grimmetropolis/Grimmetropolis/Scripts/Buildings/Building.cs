using Microsoft.Xna.Framework;

public abstract class Building : Structure
{
    public abstract ResourcePile GetResourceCost();

    public override bool CanBeAttacked => true;

    private float _health = Config.BUILDING_DEFAULT_HEALTH;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0f) TDObject?.Destroy();
        }
    }
}
