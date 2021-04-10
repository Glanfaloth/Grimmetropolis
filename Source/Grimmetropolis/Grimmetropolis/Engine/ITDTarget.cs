using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public interface ITDTarget
{
    TDObject TDObject { get; }
    Vector3 OffsetTarget { get; }
}

