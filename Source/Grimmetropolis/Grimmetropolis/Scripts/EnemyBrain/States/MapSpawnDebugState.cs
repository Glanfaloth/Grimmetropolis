using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

/// <summary>
/// This state is only used for enemy spawned by the map, so it's enough to simply send enemies to the castle.
/// </summary>
public class MapSpawnDebugState : EnemyGroupState
{
    internal override void SendCommands(EnemyGroup enemyGroup)
    {
        
        MoveCommand cmd = new MoveCommand(enemyGroup.Graph, GetArtifactLocation(), EnemyMove.Type.None);
        foreach (var enemy in enemyGroup.AllEnemies)
        {
            enemy.CurrentCommand = cmd;
            if (enemy.Items[0] is MagicalArtifact)
            {
                enemy.CurrentCommand = new MoveCommand(enemyGroup.Graph, GameManager.Instance.Map.MapTiles[0, 0].TileVertex, EnemyMove.Type.StealArtifact);
            }
        }
    }

    internal override EnemyGroupState UpdateState(EnemyGroup enemyGroup)
    {
        return this;
    }
}
