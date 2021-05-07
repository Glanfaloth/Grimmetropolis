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

    public Vector2 CenterOffset { get; set; }
    public float MaxSpeed { get; set; }
    public MovementGraph Graph { get; }

    public EnemyMove GetNextMove(Vector2 localPosition, EnemyMove.Type actions, float attackRange)
    {
        MapTile tile = Graph.Map.GetMapTile(localPosition);
        return DoGetNextMove(tile, actions, attackRange);
    }

    protected abstract EnemyMove DoGetNextMove(MapTile tile, EnemyMove.Type actions, float attackRange);
}
