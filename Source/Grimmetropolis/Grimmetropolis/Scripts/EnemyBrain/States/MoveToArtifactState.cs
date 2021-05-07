using System;
using System.Collections.Generic;
using System.Text;

public class MoveToArtifactState : EnemyGroupState
{
    private int _pathLength;
    private int _movesUntilAttack = -1;
    private EnemyMove _firstAttackMove;

    internal override void SendCommands(EnemyGroup enemyGroup)
    {
        _movesUntilAttack = -1;

        Map map = GameManager.Instance.Map;
        var path = enemyGroup.Graph.GetPathToTile(map.GetMapTile(enemyGroup.Leader.Position), map.EnemyTarget, enemyGroup.Leader.Actions, enemyGroup.Leader.AttackRange);

        Location futureTile = path[0].To;

        for (int i = 0; i < path.Count; i++)
        {
            EnemyMove nextMove = path[i];
            if (nextMove.MovementType == EnemyMove.Type.Attack)
            {
                if (_movesUntilAttack < 0)
                {
                    _movesUntilAttack = i;
                    _firstAttackMove = nextMove;
                }
            }
            if (nextMove.MovementType == EnemyMove.Type.Run)
            {
                if (i < 5 && _movesUntilAttack < 0)
                {
                    futureTile = nextMove.To;
                }
            }
        }

        _pathLength = path.Count;

        MoveCommand cmd = new MoveCommand(enemyGroup.Graph, futureTile);
        SendCommandToAll(enemyGroup, cmd);
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

