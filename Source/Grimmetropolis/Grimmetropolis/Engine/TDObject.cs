using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;

    public List<TDComponent> Components = new List<TDComponent>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        Transform = new TDTransform();
        Transform.Initialize();

        Transform.LocalPosition = localPosition;
        Transform.LocalRotation = localRotation;
        Transform.LocalScale = localScale;

        Transform.Parent = parent;

        TDSceneManager.ActiveScene.TDObjects.Add(this);
    }

    public void Update(GameTime gameTime)
    {
        foreach (TDComponent component in Components)
        {
            component.Update(gameTime);
        }
    }

    public T AddComponent<T>() where T : TDComponent, new()
    {
        T component = new T()
        {
            TDObject = this
        };
        Components.Add(component);

        component.Initialize();
        return component;
    }

    public T GetComponent<T>() where T : TDComponent
    {
        return (T)Components.Find(o => o is T);
    }
}
