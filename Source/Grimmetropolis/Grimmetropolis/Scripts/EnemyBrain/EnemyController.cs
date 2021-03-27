using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class EnemyController : TDComponent
{
    private MovementGraph _graph;

    public Map Map { get; set; }

    /*public EnemyController(Map mapComponent)
    {
        // TODO: add behaviour after artifact is stolen
        _map = mapComponent;

        _locations = new Location[_map.Width, _map.Height];
        _nextMove = new EnemyMove[_map.Width, _map.Height];
    }*/

    public override void Initialize()
    {
        base.Initialize();

        Map = GameManager.Instance?.Map ?? null;
    }

    internal EnemyMove ComputeNextMove(Vector3 localPosition, List<EnemyMove.Type> moves)
    {
        if (_graph != null)
        {
            Point tileIndex;
            if (Map.TryGetTileIndex(localPosition, out tileIndex))
            {
                return _graph.GetNextMoveFromMapTile(tileIndex);
            }
            // TODO: move to map edge?
        }

        return new RunMove(new Location(), new Location(), 0, Vector2.Zero);
    }

    internal EnemyMove GetFutureMove(EnemyMove nextMove)
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        // TODO: add monster spawning
        // TODO: do we need to rebuild the graph every cycle?
        base.Update(gameTime);

        _graph = MovementGraph.BuildGraphFromMap(Map);
        Point start = Map.GetEnemyTargetIndex();
        _graph.ComputeShortestPathToMapTile(start);
    }
}
