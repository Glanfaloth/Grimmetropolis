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

    private readonly Map _map;
    private readonly List<Location> _vertices = new List<Location>();
    private readonly Dictionary<Tuple<EnemyMove.Type, int>, PathComputation> _paths = new Dictionary<Tuple<EnemyMove.Type, int>, PathComputation>();

    private Location _startLocation;

    public int VerticesCount => _vertices.Count;


    private MovementGraph(Map map)
    {
        _map = map;
    }

    internal EnemyMove GetNextMoveFromMapTile(MapTile tile, EnemyMove.Type actions, float attackRange)
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
            path.ComputeShortestPathToMapTile(_startLocation);
        }

        return _paths[key].GetNextMoveFromMapTile(tile);
    }

    internal EnemyMove GetNextMove(EnemyMove move, EnemyMove.Type actions, float attackRange)
    {
        int pathAttackRange = (int)attackRange;
        if ((actions & EnemyMove.Type.RangedAttack) != EnemyMove.Type.RangedAttack)
        {
            pathAttackRange = 0;
        }

        var key = new Tuple<EnemyMove.Type, int>(actions, pathAttackRange);
        return _paths[key].GetNextMove(move);
    }

    internal void ComputeShortestPathToMapTile(Point start)
    {
        _startLocation = _map.MapTiles[start.X, start.Y].TileVertex;

        foreach (var path in _paths.Values)
        {
            path.ComputeShortestPathToMapTile(_startLocation);
        }
    }

    private void AddVertex(Location location)
    {
        location.Index = _vertices.Count;
        _vertices.Add(location);
    }

    private void AddMapVertices()
    {
        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                AddVertex(_map.MapTiles[x, y].TileVertex);
                AddVertex(_map.MapTiles[x, y].StructureVertex);
            }
        }
    }
}

