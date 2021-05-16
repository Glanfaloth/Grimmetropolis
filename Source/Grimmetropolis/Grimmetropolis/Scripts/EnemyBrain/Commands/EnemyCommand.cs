using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


public abstract class EnemyCommand
{
    protected EnemyCommand(MovementGraph graph)
    {
        Graph = graph;
    }

    public MovementGraph Graph { get; }

    public abstract NextMoveInfo GetNextMoveInfo(CommandCache cache, Vector2 localPosition, EnemyMove.Type actions, float attackRange);

    protected NextMoveInfo ComputePathToTile(CommandCache cache, MapTile startTile, EnemyMove.Type actions, float attackRange, Location endLocation, EnemyMove.Type endOfPathAction)
    {
        EnemyMove nextMove;
        if (cache.IsInvalid(startTile, endLocation))
        {
            nextMove = Graph.GetNextMoveFromMapTile(startTile, actions, attackRange, endLocation, endOfPathAction);
            cache.Reset(nextMove, startTile, endLocation);
        }
        else
        {
            nextMove = cache.NextMove;
        }

        return nextMove.CreateInfo(); ;
    }
}
