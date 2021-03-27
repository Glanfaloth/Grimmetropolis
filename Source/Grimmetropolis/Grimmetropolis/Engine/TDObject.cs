using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;

    public List<TDComponent> Components = new List<TDComponent>();
    /*private List<TDComponent> _components = new List<TDComponent>();
    public IReadOnlyCollection<TDComponent> Components => _components.AsReadOnly();*/


    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent, bool updateFirst)
    {
        Transform = new TDTransform()
        {
            TDObject = this
        };

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

    public void Destroy()
    {
        for (int i = Components.Count - 1; i >= 0; i--)
        {
            Components[i].Destroy();
        }

        Transform.Destroy();

        TDSceneManager.ActiveScene.TDObjects.Remove(this);
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

    public void AddComponent(TDComponent component)
    {
        component.TDObject = this;
        Components.Add(component);
        component.Initialize();
    }

    public T GetComponent<T>() where T : TDComponent
    {
        return (T)Components.Find(o => o is T);
    }
}
