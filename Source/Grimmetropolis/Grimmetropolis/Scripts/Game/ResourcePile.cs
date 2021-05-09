using Microsoft.Xna.Framework;

public class ResourcePile
{
    private static int _maxWood = int.MaxValue;
    private static int _maxStone = int.MaxValue;
    private static int _maxFood = 200;

    public int Wood = 0;
    public int Stone = 0;
    public int Food = 0;

    public ResourcePile(int wood, int stone, int food)
    {
        Wood = wood;
        Stone = stone;
        Food = food;
    }

    public ResourcePile(int wood, int stone) : this(wood, stone, 0) { }

    public ResourcePile() : this(0, 0) { }

    public static ResourcePile operator -(ResourcePile a) => new ResourcePile(-a.Wood, -a.Stone, -a.Food);
    public static ResourcePile operator +(ResourcePile a, ResourcePile b)
    {
        return Min(new ResourcePile(a.Wood + b.Wood, a.Stone + b.Stone, a.Food + b.Food), new ResourcePile(_maxWood, _maxStone, _maxFood));
    }

    public static ResourcePile operator -(ResourcePile a, ResourcePile b)
    {
        return Min(new ResourcePile(a.Wood - b.Wood, a.Stone - b.Stone, a.Food - b.Food), new ResourcePile(_maxWood, _maxStone, _maxFood));
    }

    public override string ToString() => $"Wood: {Wood}, Stone {Stone}, Food {Food}";

    public static bool CheckAvailability(ResourcePile a, ResourcePile b)
    {
        if (a.Wood - b.Wood < 0) return false;
        if (a.Stone - b.Stone < 0) return false;
        if (a.Food - b.Food < 0) return false;

        return true;
    }

    public static ResourcePile Max(ResourcePile a, ResourcePile b)
    {
        return new ResourcePile(MathHelper.Max(a.Wood, b.Wood), MathHelper.Max(a.Stone, b.Stone), MathHelper.Max(a.Food, b.Food));
    }
    public static ResourcePile Min(ResourcePile a, ResourcePile b)
    {
        return new ResourcePile(MathHelper.Min(a.Wood, b.Wood), MathHelper.Min(a.Stone, b.Stone), MathHelper.Min(a.Food, b.Food));
    }
}
