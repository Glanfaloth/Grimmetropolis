using Microsoft.Xna.Framework;

using System;

public class Structure : TDComponent
{
    public Point Position = Point.Zero;
    public Point Size = new Point(1, 1);

    public bool IsPassable = false;

    public override void Initialize()
    {
        base.Initialize();

        SetMapTransform();
        PlaceStructure(this);

        GameManager.Instance.Structures.Add(this);
    }

    public override void Destroy()
    {
        base.Destroy();

        PlaceStructure(null);
        GameManager.Instance.Structures.Remove(this);
    }

    private void SetMapTransform()
    {
        Vector3 position = GameManager.Instance.Map.MapTiles[Position.X, Position.Y].TDObject.Transform.Position + new Vector3(Size.X / 2, Size.Y / 2, 0f);
        TDObject.Transform.Position = position;
    }

    private void PlaceStructure(Structure structure)
    {
        int xHigh = Math.Clamp(Position.X + Size.X, 0, GameManager.Instance.Map.Width);
        int yHigh = Math.Clamp(Position.Y + Size.Y, 0, GameManager.Instance.Map.Height);

        for (int x = Position.X; x < xHigh; x++)
        {
            for (int y = Position.Y; y < yHigh; y++)
            {
                GameManager.Instance.Map.MapTiles[x, y].Structure = structure;
            }
        }
    }
}
