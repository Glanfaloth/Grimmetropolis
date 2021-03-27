using Microsoft.Xna.Framework;

public abstract class TDComponent
{
    public TDObject TDObject;

    public virtual void Initialize() { }

    // TODO: Are there cases where a component doesn't need update?
    public virtual void Update(GameTime gameTime) { }

    public virtual void Destroy()
    {
        TDObject.Components.Remove(this);
    }
}
