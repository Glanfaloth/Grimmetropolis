using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransformComponent Transform;

    public List<TDComponent> Components = new List<TDComponent>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDObject parent)
    {
        Transform = new TDTransformComponent(this, localPosition, localRotation, localScale, parent);

        TDSceneManager.ActiveScene.TDObjects.Add(this);
    }

    public void Update(GameTime gameTime)
    {
        foreach (TDComponent component in Components)
        {
            component.Update(gameTime);
        }
    }
}
