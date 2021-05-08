using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using static TDPriorityQueue<EnemyMove>;

public class MovementGraph
{
    internal static MovementGraph BuildGraphFromMap(Map map)
    {
        MovementGraph result = new MovementGraph(map);
        result.AddMapVertices();
        return result;
    }

    public Map Map { get; }
    private readonly List<Location> _vertices = new List<Location>();
    private readonly Dictionary<Tuple<EnemyMove.Type, int>, PathComputation> _paths = new Dictionary<Tuple<EnemyMove.Type, int>, PathComputation>();

    private Location _startLocation;

    public int VerticesCount => _vertices.Count;


    private MovementGraph(Map map)
    {
        Map = map;
    }

    internal List<EnemyMove> GetPathToTile(MapTile startTile, Point target, EnemyMove.Type actions, float attackRange)
    {
        Location targetLocation = Map.MapTiles[target.X, target.Y].TileVertex;
        PathComputation pathComputation = GetPathComputation(actions, attackRange);
        return pathComputation.GetPathToLocation(startTile, targetLocation);
    }

    internal EnemyMove GetNextMoveFromMapTile(MapTile tile, EnemyMove.Type actions, float attackRange, Location target)
    {
        PathComputation pathComputation = GetPathComputation(actions, attackRange);
        return pathComputation.GetNextMoveFromTo(tile, target);
    }

    private PathComputation GetPathComputation(EnemyMove.Type actions, float attackRange)
    {
        int pathAttackRange = (int)attackRange;
        if ((actions & EnemyMove.Type.RangedAttack) != EnemyMove.Type.RangedAttack)
        {
            pathAttackRange = 0;
        }

        var key = new Tuple<EnemyMove.Type, int>(actions, pathAttackRange);
        if (!_paths.ContainsKey(key))
        {
            PathComputation path = new PathComputation(this, actions, pathAttackRange);
            _paths[key] = path;
        }

        return _paths[key];
    }

    //internal EnemyMove GetNextMove(EnemyMove move, EnemyMove.Type actions, float attackRange)
    //{
    //    int pathAttackRange = (int)attackRange;
    //    if ((actions & EnemyMove.Type.RangedAttack) != EnemyMove.Type.RangedAttack)
    //    {
    //        pathAttackRange = 0;
    //    }

    //    var key = new Tuple<EnemyMove.Type, int>(actions, pathAttackRange);
    //    return _paths[key].GetNextMove(move);
    //}

    //internal void ComputeShortestPathToMapTile(Point start)
    //{
    //    _startLocation = Map.MapTiles[start.X, start.Y].TileVertex;

    //    foreach (var path in _paths.Values)
    //    {
    //        path.ComputeShortestPathToMapTile(_startLocation);
    //    }
    //}

    internal void ClearPaths()
    {
        foreach (var path in _paths.Values)
        {
            path.ClearPaths();
        }
    }

    private void AddVertex(Location location)
    {
        location.Index = _vertices.Count;
        _vertices.Add(location);
    }

    private void AddMapVertices()
    {
        for (int x = 0; x < Map.Width; x++)
        {
            for (int y = 0; y < Map.Height; y++)
            {
                AddVertex(Map.MapTiles[x, y].TileVertex);
                AddVertex(Map.MapTiles[x, y].StructureVertex);
            }
        }
    }
}

