using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;

    // TODO: make this readonly and add method AddComponent that makes sure references are set.
    public List<TDComponent> Components = new List<TDComponent>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent, bool updateFirst)
    {
        Transform = new TDTransform(this, localPosition, localRotation, localScale, parent);

        if (updateFirst)
        {
            TDSceneManager.ActiveScene.UpdateFirstObjects.Add(this);
        }
        else
        {
            TDSceneManager.ActiveScene.TDObjects.Add(this);
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (TDComponent component in Components)
        {
            component.Update(gameTime);
        }
    }

    public T GetComponent<T>()
    {
        object component = Components.Find(o => o is T);
        return (T)component;
    }
}
