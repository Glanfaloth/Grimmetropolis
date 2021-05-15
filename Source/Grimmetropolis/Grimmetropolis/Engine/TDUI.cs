using System;
using System.Collections.Generic;
using System.Text;

public abstract class TDUI : TDComponent
{
    public override void Initialize()
    {
        base.Initialize();

        AddToList();
    }

    private bool _isShowing = true;
    public bool IsShowing
    {
        get => _isShowing;
        set
        {
            _isShowing = value;
            AddToList();
        }
    }

    protected abstract void AddToList();
}
