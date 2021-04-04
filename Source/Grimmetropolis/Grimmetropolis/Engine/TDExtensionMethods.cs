using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public static class TDExtensionMethods
{
    public static Vector2 GetXY(this Vector3 v) => new Vector2(v.X, v.Y);
    public static Vector3 ToVector3(this Vector2 v) => new Vector3(v, 0);
    public static Vector3 ToVector3(this Point p) => new Vector3(p.X, p.Y, 0);
}
