using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using QueueEntry = System.Tuple<float, Location, EnemyMove>;

public class MovementGraph
{
    private class QueueEntryComparer : IComparer<QueueEntry>
    {
        public int Compare([AllowNull] QueueEntry x, [AllowNull] QueueEntry y)
        {
            int result = x.Item1.CompareTo(y.Item1);
            return result == 0 
                ? x.Item2.Index.CompareTo(y.Item2.Index) 
                : result;
        }
    }

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
        // TODO: Ideally, this should be a priorityqueue, however support will only be added in .NET 6.
        // src: https://github.com/dotnet/runtime/issues/43957
        // measure if this is a bottleneck
        ResetPaths();
        Location startLocation = _map.MapTiles[start.X, start.Y].TileVertex;
        var pq = new SortedSet<QueueEntry>(new QueueEntryComparer())
        {
            new QueueEntry(0f, startLocation, new StealArtifact(startLocation, startLocation)),
        };


        // for now we use a very badly implemented djikstra
        while (pq.Count > 0)
        {
            var tuple = pq.Min;
            (float distance, Location v, EnemyMove e) = tuple;
            pq.Remove(tuple);

            if (!_visited[v.Index])
            {
                _visited[v.Index] = true;
                _nextMoves[v.Index] = e;

                // we traverse graph in reverse, since we compute all path to target
                foreach (EnemyMove inEdge in v.InEdges)
                {
                    if (!_visited[inEdge.From.Index])
                    {
                        pq.Add(new QueueEntry(distance + inEdge.Cost, inEdge.From, inEdge));
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
    }
}

