using Microsoft.Xna.Framework;

public interface ITarget
{
    float Health { get; }
    TDObject TDObject { get; }
    Vector3 OffsetTarget { get; }
}

