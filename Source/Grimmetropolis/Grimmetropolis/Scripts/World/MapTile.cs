
using Microsoft.Xna.Framework;
using System;

public enum MapTileType
{
    Ground,
    Water
}

public class MapTile : TDComponent
{
    private static readonly float EDGE_COST_DIRECT = 1;
    private static readonly float EDGE_COST_DIAGONAL = (float)Math.Sqrt(2);


    public Point Position = Point.Zero;
    public MapTileType Type = MapTileType.Ground;

    public TDCuboidCollider collider;

    public bool IsPassable { get => CheckPassability(); }

    public Location TileVertex { get; } = new Location();
    public Location StructureVertex { get; } = new Location();
    private Structure _structure = null;

    public Structure Structure
    {
        get => _structure;
        set
        {
            if (_structure != value)
            {
                _structure = value;
                AdjustCollider();
                UpdateGraph();
            }
        }
    }

    public Map Map { get; internal set; }

    public override void Initialize()
    {
        base.Initialize();

        AdjustCollider();
        UpdateGraph();
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

    private void UpdateGraph()
    {
        TileVertex.ClearIncomingEdges();
        StructureVertex.ClearIncomingEdges();

        if (CheckPassability())
        {
            int x = Position.X;
            int y = Position.Y;

            // direct neighbours
            AddIncomingEdge(x - 1, y, EnemyMove.Type.Run, EDGE_COST_DIRECT);
            AddIncomingEdge(x + 1, y, EnemyMove.Type.Run, EDGE_COST_DIRECT);
            AddIncomingEdge(x, y - 1, EnemyMove.Type.Run, EDGE_COST_DIRECT);
            AddIncomingEdge(x, y + 1, EnemyMove.Type.Run, EDGE_COST_DIRECT);

            // diagonal neighbours
            bool topFree = IsTilePassable(x, y - 1);
            bool botFree = IsTilePassable(x, y + 1);
            bool leftFree = IsTilePassable(x - 1, y);
            bool rightFree = IsTilePassable(x + 1, y);
            if (leftFree && topFree) AddIncomingEdge(x - 1, y - 1, EnemyMove.Type.Run, EDGE_COST_DIAGONAL);
            if (leftFree && botFree) AddIncomingEdge(x - 1, y + 1, EnemyMove.Type.Run, EDGE_COST_DIAGONAL);
            if (rightFree && topFree) AddIncomingEdge(x + 1, y - 1, EnemyMove.Type.Run, EDGE_COST_DIAGONAL);
            if (rightFree && botFree) AddIncomingEdge(x + 1, y + 1, EnemyMove.Type.Run, EDGE_COST_DIAGONAL);
        }
        // TODO: add other edge types
    }

    private void AddIncomingEdge(int xFrom, int yFrom, EnemyMove.Type movementType, float cost)
    {
        if (!Map.IsInBounds(xFrom, yFrom))
        {
            return;
        }

        Location from = Map.MapTiles[xFrom, yFrom].TileVertex;
        switch (movementType)
        {
            case EnemyMove.Type.Run:
                // TODO: this should be done in a cleaner way
                new RunMove(from, TileVertex, cost, TDObject.Transform.Position);
                break;
            case EnemyMove.Type.Attack:
                // TODO: create attack move
                break;
            default:
                break;
        }
    }

    private bool IsTilePassable(int x, int y)
    {
        return Map.IsInBounds(x, y) && Map.MapTiles[x, y].CheckPassability();
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
