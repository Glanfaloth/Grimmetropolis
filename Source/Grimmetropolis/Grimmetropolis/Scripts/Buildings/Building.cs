using Microsoft.Xna.Framework;

public class Building : TDComponent
{
    private float _health = 3f;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0f) TDObject?.Destroy();
        }
    }

    private Point _position = Point.Zero;
    public Point Position
    {
        get => _position;
        set
        {
            _position = value;
            // SetTransform();
        }
    }

    public Point Size = new Point(1, 1);

    public bool IsPassable = false;

    public float WoodCost = 1f;
    public float StoneCost = 0f;

    public override void Initialize()
    {
        base.Initialize();

        GameManager.Instance.WoodResource -= WoodCost;
        GameManager.Instance.StoneResource -= StoneCost;

        SetTransform();
        PlaceBuilding();

        GameManager.Instance.Buildings.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        RazeBuilding();
        GameManager.Instance.Buildings.Remove(this);
    }

    private void SetTransform()
    {
        Vector3 position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position + new Vector3(Size.X / 2, Size.Y / 2, 0f);
        TDObject.Transform.Position = position;
    }

    private void PlaceBuilding()
    {
        for (int x = Position.X; x < Position.X + Size.X; x++)
        {
            for (int y = Position.Y; y < Position.Y + Size.Y; y++)
            {
                GameManager.Instance.Map.MapTiles[x, y].Building = this;
            }
        }
    }

    public void RazeBuilding()
    {
        for (int x = Position.X; x < Position.X + Size.X; x++)
        {
            for (int y = Position.Y; y < Position.Y + Size.Y; y++)
            {
                GameManager.Instance.Map.MapTiles[x, y].Building = null;
            }
        }
    }
}
