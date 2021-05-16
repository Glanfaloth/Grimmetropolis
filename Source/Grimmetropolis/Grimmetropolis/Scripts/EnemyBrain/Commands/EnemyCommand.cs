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

    private EnemyMove _currentMove;
    private MapTile _currentStartTile;
    private Location _currentEndLocation;

    private double _timeUntilRecomputePath = 0;

    public NextMoveInfo GetNextMoveInfo(GameTime gameTime, Vector2 localPosition, EnemyMove.Type actions, float attackRange)
    {
        _timeUntilRecomputePath -= gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeUntilRecomputePath <= 0)
        {
            _currentMove = null;
        }

        return DoGetNextMoveInfo(localPosition, actions, attackRange);
    }

    protected NextMoveInfo ComputePathToTile(MapTile startTile, EnemyMove.Type actions, float attackRange, Location endLocation, EnemyMove.Type endOfPathAction)
    {
        if (_currentMove == null
            || _currentMove.ShouldPathBeRecomputed()
            || _currentStartTile != startTile
            || _currentEndLocation != endLocation)
        {
            _currentMove = Graph.GetNextMoveFromMapTile(startTile, actions, attackRange, endLocation, endOfPathAction);
            _currentStartTile = startTile;
            _currentEndLocation = endLocation;
            _timeUntilRecomputePath = 2;
        }

        return _currentMove.CreateInfo(); ;
    }

    protected abstract NextMoveInfo DoGetNextMoveInfo(Vector2 localPosition, EnemyMove.Type actions, float attackRange);

    public abstract bool IsDifferent(EnemyCommand other);
}
