using Microsoft.Xna.Framework;

public class Building : Structure
{
    public static ResourcePile ResourceCost = new ResourcePile();

    private float _health = 3f;
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
