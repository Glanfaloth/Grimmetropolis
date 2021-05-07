using System;
using System.Collections.Generic;
using System.Text;


public class MoveCommand : EnemyCommand
{
    private readonly Location _targetLocation;

    public MoveCommand(MovementGraph graph, Location targetLocation) : base(graph)
    {
        _targetLocation = targetLocation;
    }

    protected override EnemyMove DoGetNextMove(MapTile tile, EnemyMove.Type actions, float attackRange)
    {
        return Graph.GetNextMoveFromMapTile(tile, actions, attackRange, _targetLocation);
    }
}
