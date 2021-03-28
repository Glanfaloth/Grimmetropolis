using Microsoft.Xna.Framework;

using System.Diagnostics;

public enum ResourceType
{
    Wood,
    Stone
}
public class Resource : TDComponent
{
    public Point Position = Point.Zero;
    public ResourceType Type = ResourceType.Wood;

    public bool IsPassable = false;

    public override void Initialize()
    {
        base.Initialize();

        SetTransform();
        PlaceResource();
    }

    public void GetResources()
    {
        switch (Type)
        {
            case ResourceType.Wood:
                GameManager.Instance.WoodResource += 1f;
                Debug.WriteLine("Wood collected to " + GameManager.Instance.WoodResource);
                break;
            case ResourceType.Stone:
                GameManager.Instance.StoneResource += 1f;
                Debug.WriteLine("Stone collected to " + GameManager.Instance.StoneResource);
                break;
        }
    }

    private void SetTransform()
    {
        TDObject.Transform.Position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position; ;
    }

    private void PlaceResource()
    {
        GameManager.Instance.Map.MapTiles[Position.X, Position.Y].Resource = this;
    }
}

