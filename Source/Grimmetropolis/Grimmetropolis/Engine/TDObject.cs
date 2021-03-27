using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;

    public IReadOnlyCollection<TDComponent> Components => _components.AsReadOnly();

    private readonly List<TDComponent> _components = new List<TDComponent>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent, bool updateFirst)
    {
        Transform = new TDTransform();
        Transform.Initialize();

        Transform.LocalPosition = localPosition;
        Transform.LocalRotation = localRotation;
        Transform.LocalScale = localScale;

        Transform.Parent = parent;

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

    public T AddComponent<T>() where T : TDComponent, new()
    {
        T component = new T()
        {
            TDObject = this
        };
        _components.Add(component);

        component.Initialize();
        return component;
    }

    public void AddComponent(TDComponent component)
    {
        component.TDObject = this;
        _components.Add(component);
        component.Initialize();
    }

    public T GetComponent<T>() where T : TDComponent
    {
        return (T)_components.Find(o => o is T);
    }
}
