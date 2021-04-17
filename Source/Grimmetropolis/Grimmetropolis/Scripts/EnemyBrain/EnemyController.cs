using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class EnemyController : TDComponent
{
    // Used for debug purposes
    private bool _alwaysShowPath = false;

    private int _witchToSpawn = 0;
    private int _knightToSpawn = 0;

    private int _siegeToSpawn = 0;

    private float _nextWaveWitch;
    private float _nextWaveKnight;
    private float _nextWaveSiege;

    private float _interval = .2f;
    private float _spawnTimer = 0f;
    private float _waveTimer = 0f;

    private float _timeBetweenWaves;
    private float _growthFactor;

    private bool _spawnLocationSet = false;

    private int _spawnLocationIndex = -1;
    private List<MapTile> _spawnTiles = new List<MapTile>();

    private List<MapTile> _highlightedPath = new List<MapTile>();

    private List<Enemy> enemies = new List<Enemy>();

    public List<Point> SpawnLocations { get; } = new List<Point>();

    private MovementGraph _graph;

    // TODO: introduce seed for rng
    // maybe create custom class in engine for providing all random numbers
    private Random rng = new Random();

    public Map Map { get; set; }

    // TODO: add behaviour after artifact is stolen

    public void Configure(float timeUntilSpawn, float timeBetweenWaves, float nrKnights, float nrWitches, float nrSiege, float growthFactor)
    {
        _waveTimer = timeUntilSpawn;
        _timeBetweenWaves = timeBetweenWaves;
        _nextWaveKnight = nrKnights;
        _nextWaveWitch = nrWitches;
        _nextWaveSiege = nrSiege;
        _growthFactor = growthFactor;
    }

    public override void Initialize()
    {
        base.Initialize();

        Map = GameManager.Instance.Map;
        _graph = MovementGraph.BuildGraphFromMap(Map);
    }

    internal EnemyMove ComputeNextMove(Vector2 localPosition, EnemyMove.Type actions, float attackRange)
    {
        MapTile tile = Map.GetMapTile(localPosition);
        return _graph.GetNextMoveFromMapTile(tile, actions, attackRange);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        UpdatePathing(gameTime);
        SpawnEnemies(gameTime);
    }

    private void SpawnEnemies(GameTime gameTime)
    {
        _waveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        _spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_waveTimer < 0f)
        {
            _spawnTimer = _waveTimer;
            _waveTimer += _timeBetweenWaves;

            _knightToSpawn += (int)_nextWaveKnight;
            _witchToSpawn += (int)_nextWaveWitch;
            _siegeToSpawn += (int)_nextWaveSiege;

            _nextWaveKnight *= _growthFactor;
            _nextWaveWitch *= _growthFactor;
            _nextWaveSiege *= _growthFactor;

            _spawnTiles = Map.GetNearbyTilesEuclidean(SpawnLocations[_spawnLocationIndex], Config.WAVE_SPAWN_RADIUS)
                .FindAll(p => p.CheckPassability());

            if (_spawnTiles.Count == 0)
            {
                _spawnTiles.Add(Map.MapTiles[SpawnLocations[_spawnLocationIndex].X, SpawnLocations[_spawnLocationIndex].Y]);
            }

            _spawnLocationSet = false;
            ClearPathHighlight();
        }

        if ((!_spawnLocationSet) && _waveTimer - Config.WAVE_ALERT_TIME < 0)
        {
            _spawnLocationIndex = (_spawnLocationIndex + 1) % SpawnLocations.Count;
            _spawnLocationSet = true;
            HightlightPath();
        }

        if (_alwaysShowPath && _spawnLocationIndex >= 0)
        {
            ClearPathHighlight();
            HightlightPath();
        }

        while (_spawnTimer < 0f && (_witchToSpawn + _knightToSpawn + _siegeToSpawn) > 0)
        {
            SpawnEnemy();
            _spawnTimer += _interval;
        }
    }

    private void ClearPathHighlight()
    {
        foreach (var tile in _highlightedPath)
        {
            tile.Darklight(false);
        }
        _highlightedPath.Clear();
    }

    private void HightlightPath()
    {
        EnemyMove.Type actions = EnemyMove.Type.Run | EnemyMove.Type.Attack;
        MapTile startTile = Map.MapTiles[SpawnLocations[_spawnLocationIndex].X, SpawnLocations[_spawnLocationIndex].Y];
        _highlightedPath.Add(startTile);
        EnemyMove nextMove = _graph.GetNextMoveFromMapTile(startTile, actions, 0);

        while (nextMove.MovementType != EnemyMove.Type.StealArtifact)
        {
            if (nextMove is RunMove runMove)
            {
                _highlightedPath.Add(runMove.Destination);
            }

            nextMove = _graph.GetNextMove(nextMove, actions, 0);
        }

        foreach (var tile in _highlightedPath)
        {
            tile.Darklight(true);
        }
    }

    private void SpawnEnemy()
    {
        int spawnTileIndex = rng.Next(_spawnTiles.Count);
        int spawnTypeIndex = rng.Next(_knightToSpawn + _witchToSpawn + _siegeToSpawn);

        Vector3 position = Map.Corner + _spawnTiles[spawnTileIndex].Position.ToVector3() + Map.Offcenter;
        TDObject enemyObject;

        if (_knightToSpawn > spawnTypeIndex)
        {
            enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyKnight>(Config.ENEMY_KNIGHTS_STATS, position, Quaternion.Identity);
            _knightToSpawn--;
        }
        else if (_witchToSpawn > spawnTypeIndex - _knightToSpawn)
        {
            enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyWitch>(Config.ENEMY_WITCH_STATS, position, Quaternion.Identity);
            _witchToSpawn--;
        }
        else if (_siegeToSpawn > spawnTypeIndex - (_knightToSpawn + _witchToSpawn))
        {
            enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyCatapult>(Config.ENEMY_CATAPULT_STATS, position, Quaternion.Identity);
            _siegeToSpawn--;
        }
        else
        {
            return;
        }
        enemies.Add(enemyObject.GetComponent<Enemy>());
    }

    private void UpdatePathing(GameTime gameTime)
    {
        Point start = Map.EnemyTarget;
        _graph.ComputeShortestPathToMapTile(start);
    }

    internal float NextFloat()
    {
        return (float)rng.NextDouble();
    }
}
