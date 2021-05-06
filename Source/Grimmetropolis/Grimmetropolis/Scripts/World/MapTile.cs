
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
            UpdateOutpostCount(_structure, value);
            _structure = value;
            AdjustCollider();
            UpdateGraph();
        }
    }

    public TDMesh Mesh;

    public Item Item = null;

    private int _nearbyOutposts = 0;
    public int NearbyOutposts
    {
        get => _nearbyOutposts;
        set
        {
            _nearbyOutposts = (value > 0) ? value : 0;
        }
    }

    public Location TileVertex { get; }
    public Location StructureVertex { get; }

    public Map Map { get; internal set; }

    public MapTile()
    {
        TileVertex = new Location(this);
        StructureVertex = new Location(this);
    }

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
            collider.Size = new Vector3(1f, 1f, 4f);
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

        int x = Position.X;
        int y = Position.Y;

        if (CheckPassability())
        {

            // direct neighbours
            AddIncomingEdge(x - 1, y, EnemyMove.Type.Run, Config.RUN_MOVE_DIRECT_BASE_COST);
            AddIncomingEdge(x + 1, y, EnemyMove.Type.Run, Config.RUN_MOVE_DIRECT_BASE_COST);
            AddIncomingEdge(x, y - 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIRECT_BASE_COST);
            AddIncomingEdge(x, y + 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIRECT_BASE_COST);

            // diagonal neighbours
            bool topFree = IsTilePassable(x, y - 1);
            bool botFree = IsTilePassable(x, y + 1);
            bool leftFree = IsTilePassable(x - 1, y);
            bool rightFree = IsTilePassable(x + 1, y);
            if (leftFree && topFree) AddIncomingEdge(x - 1, y - 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIAGONAL_BASE_COST);
            if (leftFree && botFree) AddIncomingEdge(x - 1, y + 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIAGONAL_BASE_COST);
            if (rightFree && topFree) AddIncomingEdge(x + 1, y - 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIAGONAL_BASE_COST);
            if (rightFree && botFree) AddIncomingEdge(x + 1, y + 1, EnemyMove.Type.Run, Config.RUN_MOVE_DIAGONAL_BASE_COST);
        }
        else if (CanTileBeAttacked() && Structure is Building building)
        {
            AddIncomingEdge(x - 1, y, EnemyMove.Type.Attack, Config.ATTACK_MOVE_COST);
            AddIncomingEdge(x + 1, y, EnemyMove.Type.Attack, Config.ATTACK_MOVE_COST);
            AddIncomingEdge(x, y - 1, EnemyMove.Type.Attack, Config.ATTACK_MOVE_COST);
            AddIncomingEdge(x, y + 1, EnemyMove.Type.Attack, Config.ATTACK_MOVE_COST);
            new RunMove(StructureVertex, TileVertex, Config.RUN_MOVE_DIRECT_BASE_COST, this);

            List<MapTile> nearbyTiles = Map.GetNearbyTilesEuclidean(Position, Config.MAX_RANGED_ATTACK);

            foreach (var tile in nearbyTiles)
            {
                Point delta = tile.Position - Position;
                float range = MathF.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                new RangedAttackMove(tile.TileVertex, StructureVertex, building, range, Config.RANGED_ATTACK_MOVE_BASE_COST, Config.RANGED_ATTACK_MOVE_DISTANCE_FACTOR);
            }
        }
    }

    private void UpdateOutpostCount(Structure oldStructure, Structure newStructure)
    {
        bool destroyedOutpost = oldStructure is Outpost;
        bool buildOutpost = newStructure is Outpost;

        List<MapTile> nearbyTiles = Map.GetNearbyTilesManhattan(Position, Config.ENEMY_OUTPOST_AVOIDANCE_RANGE);

        if (buildOutpost && !destroyedOutpost)
        {
            foreach (MapTile tile in nearbyTiles)
            {
                tile.NearbyOutposts++;
            }
        }
        else if (destroyedOutpost)
        {
            foreach (MapTile tile in nearbyTiles)
            {
                tile.NearbyOutposts--;
            }
        }
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
                new RunMove(from, TileVertex, cost, this);
                break;
            case EnemyMove.Type.Attack:
                new AttackMove(from, StructureVertex, cost, (Building)Structure);
                break;
            default:
                throw new NotSupportedException();
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

    public bool CanTileBeAttacked()
    {
        return Structure?.CanBeAttacked ?? false;
    }

    public void Highlight(bool highlight)
    {
        Mesh.Highlight(highlight);

        Structure?.Highlight(highlight);
        Item?.Highlight(highlight);
    }
}
