using Microsoft.Xna.Framework;

public class TDComponent
{
    public TDObject TDObject;

    public TDComponent(TDObject tdObject)
    {
        TDObject = tdObject;
    }
    public virtual void Update(GameTime gametime) { }
}
