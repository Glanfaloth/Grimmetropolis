using Microsoft.Xna.Framework;

using System;

public abstract class Structure : TDComponent
{
    private Point _position = Point.Zero;
    public virtual Point Position
    {
        get => _position;
        set
        {
            _position = value;
            if (_position.X + Size.X > GameManager.Instance.Map.Width) _position.X = GameManager.Instance.Map.Width - Size.X;
            if (_position.Y + Size.Y > GameManager.Instance.Map.Height) _position.Y = GameManager.Instance.Map.Height - Size.Y;
        }
    }

    public Point Size = new Point(1, 1);

    public bool IsPassable = false;
    public virtual bool CanBeAttacked => false;

    public bool IsPreview = false;

    public TDMesh Mesh;


    public override void Initialize()
    {
        base.Initialize();

        SetMapTransform();
        if (!IsPreview) PlaceStructure(this, null);

        GameManager.Instance.Structures.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        if (!IsPreview) PlaceStructure(null, this);
        GameManager.Instance.Structures.Remove(this);
    }

    public virtual void SetMapTransform()
    {
        Vector3 position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position + new Vector3((Size.X - 1) / 2, (Size.Y - 1) / 2, 0f);
        TDObject.Transform.Position = position;
    }

    private void PlaceStructure(Structure structure, Structure previousStructure)
    {
        int xHigh = Math.Clamp(Position.X + Size.X, 0, GameManager.Instance.Map.Width);
        int yHigh = Math.Clamp(Position.Y + Size.Y, 0, GameManager.Instance.Map.Height);

        for (int x = Position.X; x < xHigh; x++)
        {
            for (int y = Position.Y; y < yHigh; y++)
            {
                if (GameManager.Instance.Map.MapTiles[x, y].Structure != previousStructure)
                {
                    TDObject?.Destroy();
                    return;
                }
            }
        }

        for (int x = Position.X; x < xHigh; x++)
        {
            for (int y = Position.Y; y < yHigh; y++)
            {
                GameManager.Instance.Map.MapTiles[x, y].Structure = structure;
            }
        }
    }

    public virtual void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);
    }

    internal virtual bool InteractWithPlayer(GameTime gameTime, Player player)
    {
        return false;
    }
}
