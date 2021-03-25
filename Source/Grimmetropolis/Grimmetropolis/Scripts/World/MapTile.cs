
public class MapTile : TDComponent
{
    private int _x;
    private int _y;

    public MapTile(TDObject tdObject, int x, int y) : base(tdObject)
    {
        _x = x;
        _y = y;
    }
}
