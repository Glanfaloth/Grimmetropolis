using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


public abstract class EnemyGroupState
{
    protected Location GetArtifactLocation() 
    { 
        Map map = GameManager.Instance.Map;
        return map[map.EnemyTarget].TileVertex;
    }

    internal abstract void SendCommands(EnemyGroup enemyGroup);
    internal abstract EnemyGroupState UpdateState(EnemyGroup enemyGroup);

    protected void SendCommandToAll(EnemyGroup enemyGroup, EnemyCommand cmd)
    {
        foreach (var enemy in enemyGroup.AllEnemies)
        {
            enemy.CurrentCommand = cmd;
        }
    }
}
