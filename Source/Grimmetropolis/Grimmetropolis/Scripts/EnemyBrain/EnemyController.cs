using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class EnemyController : TDComponent
{
    private MovementGraph _graph;

    public Map Map { get; set; }

    // TODO: add behaviour after artifact is stolen

    public override void Initialize()
    {
        base.Initialize();

        Map = GameManager.Instance.Map;
        _graph = MovementGraph.BuildGraphFromMap(Map);
    }

    internal EnemyMove ComputeNextMove(Vector2 localPosition, List<EnemyMove.Type> moves)
    {
        MapTile tile = Map.GetMapTile(localPosition);
        return _graph.GetNextMoveFromMapTile(tile);
    }

    internal EnemyMove GetFutureMove(EnemyMove nextMove)
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: add monster spawning
        base.Update(gameTime);

        Point start = Map.EnemyTarget;
        _graph.ComputeShortestPathToMapTile(start);
    }
}
