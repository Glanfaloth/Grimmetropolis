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
        SortedList<float, Outpost> offensiveBuildings = new SortedList<float, Outpost>();
        foreach (var tile in _nearbyTiles)
        {
            if (tile.Structure is Outpost outpost)
            {
                offensiveBuildings.Add((outpost.Position.ToVector2() - enemyGroup.Leader.Position).LengthSquared(), outpost);
            }
        }

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

    internal override EnemyGroupState UpdateState(EnemyGroup enemyGroup)
    {
        if (enemyGroup.ArtifactBearer != null)
        {
            return new StealArtifactState();
        }

        return this;
    }
}

