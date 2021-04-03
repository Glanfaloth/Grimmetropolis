using Microsoft.Xna.Framework;

public class TDCuboidCollider : TDCollider
{

    private Vector3 _size;
    public Vector3 Size
    {
        get => _size;
        set
        {
            _size = value;
            UpdateColliderGeometry();
        }
    }

    private Vector3 _offset;
    public Vector3 Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            UpdateColliderGeometry();
        }
    }

    public Vector3 CuboidCornerLow { get; private set; }
    public Vector3 CuboidCornerHigh { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        UpdateColliderGeometry();
    }

    public override void UpdateColliderGeometry()
    {
        CuboidCornerLow = TDObject.Transform.Position - .5f * Vector3.Multiply(TDObject.Transform.Scale, Size) + Offset;
        CuboidCornerHigh = CuboidCornerLow + Vector3.Multiply(TDObject.Transform.Scale, Size);
    }

    public override void UpdateCollision(TDCollider collider)
    {
        switch (collider)
        {
            case TDCylinderCollider cylinder:
                CollideCylinderCuboid(cylinder, this);
                break;
        }
    }
}