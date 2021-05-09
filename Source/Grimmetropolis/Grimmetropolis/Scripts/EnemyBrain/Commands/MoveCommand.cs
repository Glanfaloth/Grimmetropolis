using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


public class MoveCommand : EnemyCommand
{
    private readonly Location _targetLocation;
    private readonly EnemyMove.Type _endOfPathAction;

    public MoveCommand(MovementGraph graph, Location targetLocation, EnemyMove.Type endOfPathAction) : base(graph)
    {
        _targetLocation = targetLocation;
        _endOfPathAction = endOfPathAction;
    }

    protected override NextMoveInfo DoGetNextMoveInfo(MapTile tile, EnemyMove.Type actions, float attackRange, Vector2 localPosition)
    {
        EnemyMove nextMove = Graph.GetNextMoveFromMapTile(tile, actions, attackRange, _targetLocation, _endOfPathAction);
        return nextMove.CreateInfo();
    }
}
