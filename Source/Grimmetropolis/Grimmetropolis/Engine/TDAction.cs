using Microsoft.Xna.Framework;

using System;
using System.Diagnostics;

public delegate void ActionProcess(float progress);
public class TDAction
{
    public TDObject TDObject;

    private float _time = 0f;
    private float _duration;
    private ActionProcess _action;
    private float _delay;
    private Action _completion;
    private bool _isRepeating;

    public TDAction(TDObject tdObject, float duration, ActionProcess action, Action completion, float delay, bool isRepeating)
    {
        TDObject = tdObject;

        _duration = duration;
        _action = action;
        _delay = delay;
        _completion = completion;
        _isRepeating = isRepeating;

        TDObject.Actions.Add(this);

        if (delay <= 0f) _action?.Invoke(0f);
    }

    public void Update(GameTime gameTime)
    {
        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_time >= _delay)
        {
            if (_time - _delay >= _duration)
            {
                if (_isRepeating) _time -= _duration;
                else Destroy();
            }

            _action?.Invoke(MathHelper.Min((_time - _delay) / _duration, 1f));
            if (_time - _delay >= _duration) _completion?.Invoke();
        }
    }

    public void Destroy()
    {
        TDObject.Actions.Remove(this);
    }
}
