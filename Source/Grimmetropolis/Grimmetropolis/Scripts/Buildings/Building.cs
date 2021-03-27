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
            if (_health < 0f) TDObject.Destroy();
        }
    }

    private Point _position = Point.Zero;
    public Point Position
    {
        get => _position;
        set
        {
            _position = value;
            SetTransform();
        }
    }

    public Point Size = new Point(1, 1);

    public override void Initialize()
    {
        base.Initialize();

        SetTransform();

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
        TDObject.Transform.Position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position;
    }

    public void PlaceBuilding()
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
