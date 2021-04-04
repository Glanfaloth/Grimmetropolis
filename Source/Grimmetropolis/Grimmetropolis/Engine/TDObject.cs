using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;
    public TDRectTransform RectTransform = null;

    public List<TDComponent> Components = new List<TDComponent>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        Transform = new TDTransform()
        {
            TDObject = this
        };

        Transform.Initialize();

        Transform.Parent = parent;

        Transform.LocalPosition = localPosition;
        Transform.LocalRotation = localRotation;
        Transform.LocalScale = localScale;

        TDSceneManager.ActiveScene.CreatedObjects.Add(this);
    }

    public void Initialize()
    {
        foreach (TDComponent component in Components)
        {
            component.Initialize();
        }

        TDSceneManager.ActiveScene.TDObjects.Add(this);
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
        if (!TDSceneManager.ActiveScene.DeletedObjects.Contains(this)) TDSceneManager.ActiveScene.DeletedObjects.Add(this);
    }

    public void DestroyComponents()
    {
        for (int i = Components.Count - 1; i >= 0; i--)
        {
            Components[i].Destroy();
        }

        Transform.Destroy();

        if (TDSceneManager.ActiveScene.TDObjects.Contains(this)) TDSceneManager.ActiveScene.TDObjects.Remove(this);
        else TDSceneManager.ActiveScene.CreatedObjects.Remove(this);
    }

    public T AddComponent<T>() where T : TDComponent, new()
    {
        T component = new T()
        {
            TDObject = this
        };
        Components.Add(component);

        return component;
    }

    public void AddComponent(TDComponent component)
    {
        component.TDObject = this;
        Components.Add(component);
    }

    public T GetComponent<T>() where T : TDComponent
    {
        return (T)Components.Find(o => o is T);
    }
}
