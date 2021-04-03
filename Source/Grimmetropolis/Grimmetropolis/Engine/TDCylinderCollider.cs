using Microsoft.Xna.Framework;

using System.Diagnostics;

public class TDCylinderCollider : TDCollider
{
    public float Radius = .5f;
    public float Height = 1f;
    public Vector3 Offset = Vector3.Zero;

    public Vector2 CenterXY { get; private set; }
    public float CenterZLow { get; private set; }
    public float CenterZHigh { get; private set; }
    public Point MapPosition { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        TDSceneManager.ActiveScene.CylinderColliderObjects.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        TDSceneManager.ActiveScene.CylinderColliderObjects.Remove(this);
    }

    public override void UpdateColliderGeometry()
    {
        Vector3 Center = TDObject.Transform.Position + Offset;
        CenterXY = new Vector2(Center.X, Center.Y);
        CenterZLow = Center.Z - .5f * TDObject.Transform.Scale.Z * Height;
        CenterZHigh = CenterZLow + TDObject.Transform.Scale.Z * Height;
        MapPosition = GameManager.Instance.Map.GetMapTile(CenterXY).Position;
    }

    public override void UpdateCollision(TDCollider collider)
    {
        switch (collider)
        {
            case TDCylinderCollider cylinder:
                CollideCylinderCylinder(this, cylinder);
                break;
            case TDCuboidCollider cuboid:
                CollideCylinderCuboid(this, cuboid);
                break;
        }
    }
}