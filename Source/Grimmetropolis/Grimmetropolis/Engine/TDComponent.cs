using Microsoft.Xna.Framework;

public abstract class TDComponent
{
    public TDObject TDObject;

    public TDComponent(TDObject tdObject)
    {
        TDObject = tdObject;
    }

    // TODO: Are there cases where a component doesn't need update?
    public virtual void Update(GameTime gameTime) { }
}
