using Microsoft.Xna.Framework;

public interface ITarget
{
    TDObject TDObject { get; }
    Vector3 OffsetTarget { get; }
}

