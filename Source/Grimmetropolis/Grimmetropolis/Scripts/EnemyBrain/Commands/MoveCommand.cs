using Microsoft.Xna.Framework;
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

    protected override NextMoveInfo DoGetNextMoveInfo(MapTile tile, EnemyMove.Type actions, float attackRange, Vector2 localPosition)
    {
        EnemyMove nextMove = Graph.GetNextMoveFromMapTile(tile, actions, attackRange, _targetLocation);
        return nextMove.CreateInfo();
    }
}
