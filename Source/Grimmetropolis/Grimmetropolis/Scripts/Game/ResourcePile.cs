public class ResourcePile
{
    public int Wood { get; set; }
    public int Stone { get; set; }

    public ResourcePile(int wood, int stone)
    {
        Wood = wood;
        Stone = stone;
    }

    public ResourcePile() : this(0, 0) { }

    public static ResourcePile operator -(ResourcePile a) => new ResourcePile(-a.Wood, -a.Stone);
    public static ResourcePile operator +(ResourcePile a, ResourcePile b) => new ResourcePile(a.Wood + b.Wood, a.Stone + b.Stone);
    public static ResourcePile operator -(ResourcePile a, ResourcePile b) => new ResourcePile(a.Wood - b.Wood, a.Stone - b.Stone);
    public override string ToString() => $"Wood: {Wood}, Stone {Stone}";

    public static bool CheckAvailability(ResourcePile a, ResourcePile b)
    {
        if (a.Wood - b.Wood < 0) return false;
        if (a.Stone - b.Stone < 0) return false;

        return true;
    }
}
