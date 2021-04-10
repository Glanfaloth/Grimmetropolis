using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

public class TDObject
{
    public TDTransform Transform;
    public TDRectTransform RectTransform = null;

    public List<TDComponent> Components = new List<TDComponent>();
    public List<TDAction> Actions = new List<TDAction>();

    public TDObject(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, TDTransform parent)
    {
        Transform = new TDTransform()
        {
            TDObject = this
        };

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

    public void UpdateActions(GameTime gameTime)
    {
        for (int i = Actions.Count - 1; i >= 0; i--)
        {
            Actions[i].Update(gameTime);
        }
    }

    public void UpdateComponents(GameTime gameTime)
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

    public void DestroyAttachedObjects()
    {
        RemoveActions();

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

    public TDAction RunAction(float duration, ActionProcess action, Action completion = null, float delay = 0f, bool isRepeating = false)
    {
        return new TDAction(this, duration, action, completion, delay, isRepeating);
    }

    public TDAction RunAction(float duration, ActionProcess action, float delay, bool isRepeating = false)
    {
        return RunAction(duration, action, null, delay, isRepeating);
    }

    public TDAction RunAction(float duration, ActionProcess action, bool isRepeating)
    {
        return RunAction(duration, action, null, 0f, isRepeating);
    }

    public void RemoveActions()
    {
        for (int i = Actions.Count - 1; i >= 0; i--)
        {
            Actions[i].Destroy();
        }
    }
    public void RemoveAction(TDAction action)
    {
        action.Destroy();
    }
}
