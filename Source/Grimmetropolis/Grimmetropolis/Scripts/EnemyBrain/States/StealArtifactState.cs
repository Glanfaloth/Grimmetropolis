using System;
using System.Collections.Generic;
using System.Text;


class StealArtifactState : EnemyGroupState
{
    internal override void SendCommands(EnemyGroup enemyGroup)
    {
        //TODO: enemies get stuck together when there is a large amount of them

        if (enemyGroup.ArtifactBearer == null)
            return;

        Location artifactLocation = GameManager.Instance.Map.GetMapTile(enemyGroup.ArtifactBearer.Position).TileVertex;

        EnemyCommand cmd = new MoveCommand(enemyGroup.Graph, artifactLocation, EnemyMove.Type.None);
        SendCommandToAll(enemyGroup, cmd);
        enemyGroup.ArtifactBearer.CurrentCommand = new MoveCommand(enemyGroup.Graph, enemyGroup.SpawnPoint.TileVertex, EnemyMove.Type.StealArtifact);
    }

    internal override EnemyGroupState UpdateState(EnemyGroup enemyGroup)
    {
        if (enemyGroup.ArtifactBearer == null)
        {
            return new MoveToArtifactState();
        }
        return this;
    }
}

