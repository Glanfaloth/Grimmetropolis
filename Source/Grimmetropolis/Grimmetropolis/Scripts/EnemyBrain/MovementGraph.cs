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
        result.ResetPaths();
        return result;
    }

    private readonly Map _map;

    private readonly List<Location> vertices = new List<Location>();

    // TODO: this doesn't support different action types of the enemies for now.
    private EnemyMove[] _nextMoves;
    private bool[] _visited;
    private Handle[] _handles;

    private MovementGraph(Map map)
    {
        _map = map;
    }

    internal EnemyMove GetNextMoveFromMapTile(MapTile tile)
    {
        // TODO: what to do when no move is set
        // TODO: sanitize input
        return _nextMoves[tile.TileVertex.Index] ?? EnemyMove.NONE;
    }

    internal void ComputeShortestPathToMapTile(Point start)
    {
        // TODO: this is currently a bottleneck
        ResetPaths();
        Location startLocation = _map.MapTiles[start.X, start.Y].TileVertex;

        var pq = new TDPriorityQueue<EnemyMove>();
        pq.Insert(0f, new StealArtifact(startLocation));


        // for now we use a very badly implemented djikstra
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
                    int u = inEdge.From.Index;
                    if (!_visited[u])
                    {
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
        // TODO: 
    }

    private void AddVertex(Location location)
    {
        location.Index = vertices.Count;
        vertices.Add(location);
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

    private void ResetPaths()
    {
        _nextMoves = new EnemyMove[vertices.Count];
        _visited = new bool[vertices.Count];
        _handles = new Handle[vertices.Count];
    }
}

