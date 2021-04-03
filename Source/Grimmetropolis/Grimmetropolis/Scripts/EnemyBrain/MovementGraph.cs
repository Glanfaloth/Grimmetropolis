using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using QueueEntry = System.Tuple<float, Location, EnemyMove>;

public class MovementGraph
{
    private static readonly float SQRT2 = (float)Math.Sqrt(2);

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
        result.CreateEdges();
        result.ResetPaths();
        return result;
    }

    private readonly Map _map;
    private readonly Location[,] _locations;
    private readonly Location _outsideTheMap;

    private readonly List<Location> vertices = new List<Location>();

    // TODO: this doesn't support different action types of the enemies for now.
    private EnemyMove[] _nextMoves;
    private bool[] _visited;

    private MovementGraph(Map map)
    {
        _map = map;
        _locations = new Location[_map.Width, _map.Height];
        _outsideTheMap = new Location();
        AddVertex(_outsideTheMap);
    }

    internal EnemyMove GetNextMoveFromMapTile(MapTile tile)
    {
        // TODO: what to do when no move is set
        // TODO: sanitize input
        return _nextMoves[_locations[tile.Position.X, tile.Position.Y].Index] ?? EnemyMove.NONE;
    }

    internal void ComputeShortestPathToMapTile(Point start)
    {
        // TODO: Ideally, this should be a priorityqueue, however support will only be added in .NET 6.
        // src: https://github.com/dotnet/runtime/issues/43957
        // measure if this is a bottleneck
        ResetPaths();
        Location startLocation = _locations[start.X, start.Y];
        var pq = new SortedSet<QueueEntry>(new QueueEntryComparer())
        {
            new QueueEntry(0f, startLocation, new StealArtifact(startLocation, startLocation, 0)),
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
                var location = new Location();
                AddVertex(location);
                _locations[x, y] = location;
            }
        }
    }

    private void CreateEdges()
    {
        // this needs to be two loops
        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                MapTile tile = _map.MapTiles[x, y];

                if (tile.CheckPassability())
                {
                    Location to = _locations[x, y];

                    // direct neighbours
                    AddEdge(x - 1, y, to, tile, EnemyMove.Type.Run, 1);
                    AddEdge(x + 1, y, to, tile, EnemyMove.Type.Run, 1);
                    AddEdge(x, y - 1, to, tile, EnemyMove.Type.Run, 1);
                    AddEdge(x, y + 1, to, tile, EnemyMove.Type.Run, 1);

                    // diagonal neighbours
                    // TODO: those might not work in all case, for example this one:
                    // 1, 0
                    // 0, 1
                    bool topFree = IsTilePassable(x, y - 1);
                    bool botFree = IsTilePassable(x, y + 1);
                    bool leftFree = IsTilePassable(x - 1, y);
                    bool rightFree = IsTilePassable(x + 1, y);
                    if(leftFree && topFree) AddEdge(x - 1, y - 1, to, tile, EnemyMove.Type.Run, SQRT2);
                    if(leftFree && botFree) AddEdge(x - 1, y + 1, to, tile, EnemyMove.Type.Run, SQRT2);
                    if(rightFree && topFree) AddEdge(x + 1, y - 1, to, tile, EnemyMove.Type.Run, SQRT2);
                    if(rightFree && botFree) AddEdge(x + 1, y + 1, to, tile, EnemyMove.Type.Run, SQRT2);
                }
                // TODO: add other edge types
            }
        }
    }

    private bool IsTilePassable(int x, int y)
    {
        return _map.IsInBounds(x, y) && _map.MapTiles[x, y].CheckPassability();
    }

    private void AddEdge(int xFrom, int yFrom, Location to, MapTile tile, EnemyMove.Type movementType, float cost)
    {
        Location from = _outsideTheMap;
        if (_map.IsInBounds(xFrom, yFrom))
        {
            from = _locations[xFrom, yFrom];
        }


        switch (movementType)
        {
            case EnemyMove.Type.Run:
                // TODO: this should be done in a cleaner way
                new RunMove(from, to, cost, tile.TDObject.Transform.Position);
                break;
            case EnemyMove.Type.Attack:
                // TODO: create attack move
                break;
            default:
                break;
        }
    }

    private void ResetPaths()
    {
        _nextMoves = new EnemyMove[vertices.Count];
        _visited = new bool[vertices.Count];
    }
}

