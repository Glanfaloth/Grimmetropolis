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

    internal PathComputation(MovementGraph graph, EnemyMove.Type actions, int attackRange)
    {
        _graph = graph;
        _actions = actions;
        _attackRange = attackRange;
    }

    internal EnemyMove GetNextMoveFromMapTile(MapTile tile)
    {
        return _nextMoves[tile.TileVertex.Index] ?? EnemyMove.NONE;
    }

    internal void ComputeShortestPathToMapTile(Location startLocation)
    {
        // TODO: this function is currently a bottleneck for large maps
        ResetPaths();

        var pq = new TDPriorityQueue<EnemyMove>();
        pq.Insert(0f, new StealArtifact(startLocation));

        // for now we use a djikstra
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

                // we traverse graph in reverse, since we compute all path to target
                foreach (EnemyMove inEdge in v.InEdges)
                {
                    if (inEdge.IsMoveAllowed(_actions, _attackRange))
                    {
                        int u = inEdge.From.Index;
                        if (!_visited[u])
                        {
                            // TODO: add weight factor depending on enemy type
                            float futureCost = distance + inEdge.Cost;
                            // _handles[u] = pq.Insert(futureCost, inEdge);
                            if (_handles[u] == null)
                            {
                                _handles[u] = pq.Insert(futureCost, inEdge);
                            }
                            else if (_handles[u].Cost > futureCost)
                            {
                                pq.DecreaseCost(_handles[u], futureCost, inEdge);
                            }
                        }
                    }
                }
            }
        }
    }

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

