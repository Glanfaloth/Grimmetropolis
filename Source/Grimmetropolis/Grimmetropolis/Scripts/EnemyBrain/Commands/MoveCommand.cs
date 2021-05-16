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

    public override NextMoveInfo GetNextMoveInfo(CommandCache cache, Vector2 localPosition, EnemyMove.Type actions, float attackRange)
    {
        MapTile tile = Graph.Map.GetMapTile(localPosition);
        return ComputePathToTile(cache, tile, actions, attackRange, _targetLocation, _endOfPathAction);
    }
}
