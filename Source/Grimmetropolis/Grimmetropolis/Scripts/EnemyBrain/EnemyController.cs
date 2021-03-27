using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class EnemyController : TDComponent
{
    private Map _map;
    public Map Map
    {
        get => _map;
        set
        {
            _map = value;

            if (_map != null)
            {
                _locations = new Location[_map.Width, _map.Height];
                _nextMove = new EnemyMove[_map.Width, _map.Height];
            }
        }
    }

    private Location[,] _locations;
    private Location _outsideTheMap;

    private readonly float SQRT2 = (float)Math.Sqrt(2);

    // TODO: this doesn't support different action types of the enemies for now.
    private EnemyMove[,] _nextMove;

    /*public EnemyController(Map mapComponent)
    {
        // TODO: add behaviour after artifact is stolen
        _map = mapComponent;

        _locations = new Location[_map.Width, _map.Height];
        _nextMove = new EnemyMove[_map.Width, _map.Height];
    }*/

    public override void Initialize()
    {
        base.Initialize();

        Map = GameManager.Instance?.Map ?? null;
    }

    internal EnemyMove ComputeNextMove(Vector3 localPosition, List<EnemyMove.Type> moves)
    {
        return new RunMove(new Location(), new Location(), 0, Vector2.Zero);
    }

    internal EnemyMove GetFutureMove(EnemyMove nextMove)
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: add monster spawning
        // TODO: do we need to rebuild the graph every cycle?
        base.Update(gameTime);

        RebuildGraph();
        ComputeIdealPath();
    }

    private void ComputeIdealPath()
    {
        Point start = _map.GetEnemyTarget();
        // TODO: 
    }

    private void RebuildGraph()
    {
        _outsideTheMap = new Location();
        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                _locations[x, y] = new Location();

                MapTile tile = _map.mapTiles[x, y];

                if (tile.CanEnemyMoveThrough())
                {
                    Location to = _locations[x, y];

                    AddEdge(x, y - 1, to, tile, EnemyMove.Type.Run, 1);
                    AddEdge(x - 1, y - 1, to, tile, EnemyMove.Type.Run, SQRT2);
                    AddEdge(x - 1, y, to, tile, EnemyMove.Type.Run, 1);
                    AddEdge(x - 1, y + 1, to, tile, EnemyMove.Type.Run, SQRT2);
                }
                // TODO: add other edge types
            }
        }
    }

    private void AddEdge(int xFrom, int yFrom, Location to, MapTile tile, EnemyMove.Type movementType, float cost)
    {
        Location from = _outsideTheMap;
        if(xFrom >= 0 && yFrom >= 0 && xFrom < _map.Width && yFrom < _map.Height)
        {
            from = _locations[xFrom, yFrom];
        }
        switch (movementType)
        {
            case EnemyMove.Type.Run:
                new RunMove(from, to, cost, tile.Position);
                break;
            case EnemyMove.Type.Attack:
                // TODO: create attack move
                break;
            default:
                break;
        }
    }
}
