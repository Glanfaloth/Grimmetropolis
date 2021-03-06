using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


public class AttackCastleState : EnemyGroupState
{
    private List<MapTile> _nearbyTiles = null;

    internal override void SendCommands(EnemyGroup enemyGroup)
    {
        Map map = GameManager.Instance.Map;
        if (_nearbyTiles == null)
        {
            Vector2 artifactLocation = map[map.EnemyTarget].TDObject.Transform.LocalPosition.GetXY();
            Vector2 toArtifact = artifactLocation - enemyGroup.Leader.Position;
            toArtifact.Normalize();
            Point referencePoint = map.GetMapTile(artifactLocation - 2 * toArtifact).Position;

            _nearbyTiles = map.GetNearbyTilesEuclidean(referencePoint, Config.COMMAND_PROXIMITY_RANGE);
        }

        // TODO: how to handle walls
        // TODO: use a sorted list that allows duplicate keys
        SortedList<float, Outpost> offensiveBuildings = new SortedList<float, Outpost>();
        foreach (var tile in _nearbyTiles)
        {
            if (tile.Structure is Outpost outpost)
            {
                float key = (outpost.Position.ToVector2() - enemyGroup.Leader.Position).LengthSquared();
                if (!offensiveBuildings.ContainsKey(key))
                {
                    offensiveBuildings.Add(key, outpost);
                }
            }
        }

        if (map.MagicalArtifact.Character is Player player)
        {
            SendCommandToAll(enemyGroup, new AttackPlayerCommand(enemyGroup.Graph, player));

            EnemyCommand catapultCommand;
            if (offensiveBuildings.Count > 0 && !map[map.EnemyTarget].IsPassable)
            {
                catapultCommand = new MoveCommand(enemyGroup.Graph, map[offensiveBuildings.Values[0].Position].TileVertex, EnemyMove.Type.None);
            }
            else
            {
                catapultCommand = new MoveCommand(enemyGroup.Graph, map[map.EnemyTarget].TileVertex, EnemyMove.Type.PickUpArtifact);
            }

            foreach (var catapult in enemyGroup.Catapults)
            {
                catapult.CurrentCommand = catapultCommand;
            }
        }
        else
        {
            // TODO: split up large groups
            if (offensiveBuildings.Count > 0 && !map[map.EnemyTarget].IsPassable)
            {
                SendCommandToAll(enemyGroup, new MoveCommand(enemyGroup.Graph, map[offensiveBuildings.Values[0].Position].TileVertex, EnemyMove.Type.None));
            }
            else
            {
                SendCommandToAll(enemyGroup, new MoveCommand(enemyGroup.Graph, map[map.EnemyTarget].TileVertex, EnemyMove.Type.PickUpArtifact));
            }
        }

    }

    internal override EnemyGroupState UpdateState(EnemyGroup enemyGroup)
    {
        if (enemyGroup.ArtifactBearer != null)
        {
            return new StealArtifactState();
        }

        return this;
    }
}

