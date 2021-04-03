
using Microsoft.Xna.Framework;
using System;

public enum MapTileType
{
    Ground,
    Water
}

public class MapTile : TDComponent
{
    public Point Position = Point.Zero;
    public MapTileType Type = MapTileType.Ground;

    public TDCuboidCollider collider;

    public bool IsPassable { get => CheckPassability(); }

    private Structure _structure = null;
    public Structure Structure
    {
        get => _structure;
        set
        {
            _structure = value;
            AdjustCollider();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        AdjustCollider();
    }

    public override void Destroy()
    {
        collider = null;

        base.Destroy();
    }

    private void AdjustCollider()
    {
        if (Structure != null)
        {
            collider.Size = new Vector3(1f, 1f, 2f);
            collider.Offset = Vector3.Zero;
        }
        else
        {
            switch (Type)
            {
                case MapTileType.Ground:
                    collider.Size = Vector3.One;
                    collider.Offset = -.5f * Vector3.Backward;
                    break;
                case MapTileType.Water:
                    collider.Size = new Vector3(1f, 1f, 2f);
                    collider.Offset = Vector3.Zero;
                    break;
            }
        }
    }

    public bool CheckPassability()
    {
        if (Structure != null)
        {
            return Structure.IsPassable;
        }

        return Type switch
        {
            MapTileType.Ground => true,
            MapTileType.Water => false,
            _ => true
        };
    }
}
