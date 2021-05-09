using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public class AttackPlayerCommand : EnemyCommand
{
    private readonly Player _target;

    public AttackPlayerCommand(MovementGraph graph, Player target) : base(graph)
    {
        _target = target;
    }

    protected override NextMoveInfo DoGetNextMoveInfo(MapTile tile, EnemyMove.Type actions, float attackRange, Vector2 localPosition)
    {
        Vector2 targetPosition = _target.TDObject.Transform.LocalPosition.GetXY();
        Vector2 toTarget = targetPosition - localPosition;
        MapTile targetTile = Graph.Map.GetMapTile(targetPosition);

        if ((actions & EnemyMove.Type.RangedAttack) == EnemyMove.Type.RangedAttack)
        {
            if (toTarget.LengthSquared() <= attackRange * attackRange)
            {
                return new NextMoveInfo(_target, EnemyMove.Type.RangedAttack, targetPosition);
            }
            else
            {
                EnemyMove nextMove = Graph.GetNextMoveFromMapTile(tile, actions, attackRange, targetTile.TileVertex, EnemyMove.Type.None);
                return nextMove.CreateInfo();
            }
        }
        else
        {
            // TODO: how close do enemy need to be?
            if (toTarget.LengthSquared() <= 0.25 * 0.25)
            {
                return new NextMoveInfo(_target, EnemyMove.Type.Attack, targetPosition);
            }
            else
            {
                EnemyMove nextMove = Graph.GetNextMoveFromMapTile(tile, actions, attackRange, targetTile.TileVertex, EnemyMove.Type.None);
                return nextMove.CreateInfo();
            }
        }
    }
}
