using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using static TDPriorityQueue<EnemyMove>;

public class PathComputation
{
    private readonly MovementGraph _graph;
    private readonly EnemyMove.Type _actions;
    private readonly int _attackRange;

    private EnemyMove[] _nextMoves;
    private bool[] _visited;
    private Handle[] _handles;

    private Dictionary<Tuple<MapTile, Location>, EnemyMove> _moveCache = new Dictionary<Tuple<MapTile, Location>, EnemyMove>();

    internal PathComputation(MovementGraph graph, EnemyMove.Type actions, int attackRange)
    {
        _graph = graph;
        _actions = actions;
        _attackRange = attackRange;
    }

    internal void ClearPaths()
    {
        _moveCache.Clear();
    }

    internal EnemyMove GetNextMoveFromTo(MapTile from, Location target)
    {
        Tuple<MapTile, Location> key = new Tuple<MapTile, Location>(from, target);

        if (!_moveCache.ContainsKey(key))
        {
            _moveCache[key] = DoComputeNextMoveFromTo(from, target);
        }

        return _moveCache[key];
    }

    private EnemyMove DoComputeNextMoveFromTo(MapTile from, Location target)
    {
        ResetPaths();

        var pq = new TDPriorityQueue<EnemyMove>();
        pq.Insert(0f, 0f, new EndOfPath(target));

        while (!pq.IsEmpty())
        {
            Handle h = pq.ExtractMin();

            float distance = h.Cost;
            Location v = h.Value.From;
            EnemyMove e = h.Value;

            if (!_visited[v.Index])
            {
                _visited[v.Index] = true;
                _nextMoves[v.Index] = e;

                if (v.Tile == from)
                {
                    return e;
                }

                // we traverse graph in reverse
                foreach (EnemyMove inEdge in v.InEdges)
                {
                    if (inEdge.IsMoveAllowed(_actions, _attackRange))
                    {
                        int u = inEdge.From.Index;
                        if (!_visited[u])
                        {
                            // TODO: add weight factor depending on enemy type
                            float costPath = distance + inEdge.Cost;
                            // _handles[u] = pq.Insert(futureCost, inEdge);
                            if (_handles[u] == null)
                            {
                                int dx = v.Tile.Position.X - from.Position.X;
                                int dy = v.Tile.Position.Y - from.Position.Y;

                                float costEstimation = MathF.Sqrt(dx * dx + dy * dy);

                                _handles[u] = pq.Insert(costPath, costEstimation, inEdge);
                            }
                            else if (_handles[u].CostPath > costPath)
                            {
                                pq.DecreaseCostPath(_handles[u], costPath, inEdge);
                            }
                        }
                    }
                }
            }
        }

        return EnemyMove.NONE;
    }

    internal List<EnemyMove> GetPathToLocation(MapTile startTile, Location targetLocation)
    {
        var moves = new List<EnemyMove>();
        EnemyMove previousMove = null;
        EnemyMove nextMove = DoComputeNextMoveFromTo(startTile, targetLocation);

        // last move is a loop
        while(nextMove != null && nextMove != previousMove)
        {
            moves.Add(nextMove);
            previousMove = nextMove;
            nextMove = _nextMoves[nextMove.To.Index];
        }

        return moves;
    }

    //internal EnemyMove GetNextMoveFromMapTile(MapTile tile)
    //{
    //    return _nextMoves[tile.TileVertex.Index] ?? EnemyMove.NONE;
    //}

    //internal void ComputeShortestPathToMapTile(Location target)
    //{
    //    // TODO: this function is currently a bottleneck for large maps
    //    ResetPaths();

    //    var pq = new TDPriorityQueue<EnemyMove>();
    //    pq.Insert(0f, 0f, new TakeArtifact(target));

    //    // for now we use a djikstra
    //    while (!pq.IsEmpty())
    //    {
    //        Handle h = pq.ExtractMin();

    //        float distance = h.Cost;
    //        Location v = h.Value.From;
    //        EnemyMove e = h.Value;

    //        if (!_visited[v.Index])
    //        {
    //            _visited[v.Index] = true;
    //            _nextMoves[v.Index] = e;

    //            // we traverse graph in reverse, since we compute all path to target
    //            foreach (EnemyMove inEdge in v.InEdges)
    //            {
    //                if (inEdge.IsMoveAllowed(_actions, _attackRange))
    //                {
    //                    int u = inEdge.From.Index;
    //                    if (!_visited[u])
    //                    {
    //                        // TODO: add weight factor depending on enemy type
    //                        float futureCost = distance + inEdge.Cost;
    //                        // _handles[u] = pq.Insert(futureCost, inEdge);
    //                        if (_handles[u] == null)
    //                        {
    //                            _handles[u] = pq.Insert(futureCost, 0, inEdge);
    //                        }
    //                        else if (_handles[u].Cost > futureCost)
    //                        {
    //                            pq.DecreaseCostPath(_handles[u], futureCost, inEdge);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //internal EnemyMove GetNextMove(EnemyMove move)
    //{
    //    return _nextMoves[move.To.Index];
    //}

    private void ResetPaths()
    {
        if ((_nextMoves?.Length ?? -1) < _graph.VerticesCount)
        {
            _nextMoves = new EnemyMove[_graph.VerticesCount];
            _visited = new bool[_graph.VerticesCount];
            _handles = new Handle[_graph.VerticesCount];
        }
        else
        {
            for (int i = 0; i < _nextMoves.Length; i++)
            {
                _nextMoves[i] = null;
                _visited[i] = false;
                _handles[i] = null;
            }
        }
    }
}

