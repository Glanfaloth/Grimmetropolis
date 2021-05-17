using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

public class CommandCache
{
    private MapTile _currentStartTile;
    private Location _currentEndLocation;

    private double _timeUntilRecomputePath = 0;

    public EnemyMove NextMove { get; private set; }

    internal bool IsInvalid(MapTile startTile, Location endLocation)
    {
        return NextMove == null
                || _timeUntilRecomputePath <= 0
                || NextMove.ShouldPathBeRecomputed()
                || _currentStartTile != startTile
                || _currentEndLocation != endLocation;
    }

    internal void Reset(EnemyMove nextMove, MapTile startTile, Location endLocation)
    {
        NextMove = nextMove;
        _currentStartTile = startTile;
        _currentEndLocation = endLocation;
        _timeUntilRecomputePath = 1;
    }

    internal void Update(GameTime gameTime)
    {
        _timeUntilRecomputePath -= gameTime.ElapsedGameTime.TotalSeconds;
    }
}
