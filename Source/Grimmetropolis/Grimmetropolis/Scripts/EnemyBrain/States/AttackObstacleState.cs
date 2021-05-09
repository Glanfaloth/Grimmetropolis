using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public class AttackObstacleState : EnemyGroupState
{
    private readonly MapTile _obstacleTile;
    private readonly List<MapTile> _outsideTiles = new List<MapTile>();
    private readonly List<MapTile> _insideTiles = new List<MapTile>();
    private readonly List<MapTile> _wallTiles = new List<MapTile>();

    private bool _hasOutpostOutside = true;

    private enum Label
    {
        None = 0,
        Wall,
        Outside,
        Inside
    }

    public AttackObstacleState(MapTile obstacleTile)
    {
        _obstacleTile = obstacleTile;
    }

    internal override void SendCommands(EnemyGroup enemyGroup)
    {
        Map map = GameManager.Instance.Map;

        if (_outsideTiles.Count == 0)
        {
            MapTile[,] nearbyTiles = map.GetNearbyTilesSquare(_obstacleTile.Position, Config.COMMAND_PROXIMITY_RANGE);

            Vector2 leaderPosition = enemyGroup.Leader.Position;
            Vector2 obstaclePosition = _obstacleTile.TDObject.Transform.LocalPosition.GetXY();
            Vector2 offset = leaderPosition - obstaclePosition;

            int mid = nearbyTiles.GetLength(0) / 2;
            Point center = new Point(mid, mid);
            Point startOutside = new Point(MathHelper.Clamp((int)offset.X + mid, 0, nearbyTiles.GetLength(0)),
                                           MathHelper.Clamp((int)offset.Y + mid, 0, nearbyTiles.GetLength(1)));

            Label[,] label = new Label[nearbyTiles.GetLength(0), nearbyTiles.GetLength(1)];
            Queue<Point> queue = new Queue<Point>();

            // mark wall
            if (_obstacleTile.Structure != null)
            {
                queue.Enqueue(center);
                while (queue.Count > 0)
                {
                    Point index = queue.Dequeue();
                    if (IsWallTile(index, nearbyTiles, label))
                    {
                        label[index.X, index.Y] = Label.Wall;
                        _wallTiles.Add(nearbyTiles[index.X, index.Y]);
                        queue.Enqueue(new Point(index.X-1, index.Y-1));
                        queue.Enqueue(new Point(index.X-1, index.Y));
                        queue.Enqueue(new Point(index.X-1, index.Y+1));

                        queue.Enqueue(new Point(index.X,   index.Y-1));
                        queue.Enqueue(new Point(index.X,   index.Y+1));

                        queue.Enqueue(new Point(index.X+1, index.Y-1));
                        queue.Enqueue(new Point(index.X+1, index.Y));
                        queue.Enqueue(new Point(index.X+1, index.Y+1));
                    }
                    
                }
            }

            // mark outside
            queue.Enqueue(startOutside);
            while (queue.Count > 0)
            {
                Point index = queue.Dequeue();
                if (IsTileOutside(index, nearbyTiles, label))
                {
                    label[index.X, index.Y] = Label.Outside;
                    _outsideTiles.Add(nearbyTiles[index.X, index.Y]);
                    queue.Enqueue(new Point(index.X - 1, index.Y));
                    queue.Enqueue(new Point(index.X + 1, index.Y));

                    queue.Enqueue(new Point(index.X, index.Y - 1));
                    queue.Enqueue(new Point(index.X, index.Y + 1));
                }
            }

            // TODO inside tiles
            //// mark inside (ignore tiles behind water)
            //queue.Enqueue(startOutside);
            //while (queue.Count > 0)
            //{
            //    Point index = queue.Dequeue();
            //    if (IsWallTile(index, nearbyTiles, label))
            //    {
            //        label[index.X, index.Y] = Label.Wall;
            //        _wallTiles.Add(nearbyTiles[index.X, index.Y]);
            //        queue.Enqueue(new Point(index.X - 1, index.Y - 1));
            //        queue.Enqueue(new Point(index.X - 1, index.Y));
            //        queue.Enqueue(new Point(index.X - 1, index.Y + 1));

            //        queue.Enqueue(new Point(index.X, index.Y - 1));
            //        queue.Enqueue(new Point(index.X, index.Y + 1));

            //        queue.Enqueue(new Point(index.X + 1, index.Y - 1));
            //        queue.Enqueue(new Point(index.X + 1, index.Y));
            //        queue.Enqueue(new Point(index.X + 1, index.Y + 1));
            //    }
            //}
        }


        // 1. attack outpost outside
        // 2. attack outpost in wall
        // 3. breach wall (siege attack outpost behind wall)

        SortedList<float, Outpost> offensiveBuildings = new SortedList<float, Outpost>();
        foreach (var tile in _outsideTiles)
        {
            if (tile.Structure is Outpost outpost)
            {
                offensiveBuildings.Add((outpost.Position.ToVector2() - enemyGroup.Leader.Position).LengthSquared(), outpost);
            }
        }

        // TODO: split up large groups
        if (offensiveBuildings.Count > 0)
        {
            SendCommandToAll(enemyGroup, new MoveCommand(enemyGroup.Graph, map[offensiveBuildings.Values[0].Position].TileVertex));
        }
        else
        {
            foreach (var tile in _wallTiles)
            {
                if (tile.Structure is Outpost outpost)
                {
                    offensiveBuildings.Add((outpost.Position.ToVector2() - enemyGroup.Leader.Position).LengthSquared(), outpost);
                }
            }

            if (offensiveBuildings.Count > 0)
            {
                SendCommandToAll(enemyGroup, new MoveCommand(enemyGroup.Graph, map[offensiveBuildings.Values[0].Position].TileVertex));
            }
            else
            {
                _hasOutpostOutside = false;
                SendCommandToAll(enemyGroup, new MoveCommand(enemyGroup.Graph, _obstacleTile.TileVertex));
            }
        }
    }

    private bool IsTileOutside(Point index, MapTile[,] nearbyTiles, Label[,] label)
    {
        if (index.X < 0
            || index.Y < 0
            || index.X >= nearbyTiles.GetLength(0)
            || index.Y >= nearbyTiles.GetLength(1)
            || label[index.X, index.Y] != Label.None)
        {
            return false;
        }

        MapTile tile = nearbyTiles[index.X, index.Y];
        return tile.IsPassable || tile.CanTileBeAttacked();
    }

    private bool IsWallTile(Point index, MapTile[,] nearbyTiles, Label[,] label)
    {
        if (index.X < 0 
            || index.Y < 0 
            || index.X >= nearbyTiles.GetLength(0)
            || index.Y >= nearbyTiles.GetLength(1)
            || label[index.X, index.Y] != Label.None)
        {
            return false;
        }

        MapTile tile = nearbyTiles[index.X, index.Y];

        // only attackable blocking tiles are considered wall
        return tile.CanTileBeAttacked() && !tile.IsPassable;
    }

    internal override EnemyGroupState UpdateState(EnemyGroup enemyGroup)
    {
        if (_hasOutpostOutside == false && _obstacleTile.IsPassable)
        {
            return new MoveToArtifactState();
        }

        return this;
    }
}
