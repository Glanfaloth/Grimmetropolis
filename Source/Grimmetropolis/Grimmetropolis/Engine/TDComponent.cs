using Microsoft.Xna.Framework;

public abstract class TDComponent
{
    public TDObject TDObject;

    public virtual void Initialize() { }

    public virtual void Update(GameTime gameTime) { }
}
