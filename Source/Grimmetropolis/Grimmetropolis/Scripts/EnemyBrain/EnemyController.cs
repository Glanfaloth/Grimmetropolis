using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class EnemyController : TDComponent
{
    public List<EnemyGroup> Groups = new List<EnemyGroup>();

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

    public List<Point> SpawnLocations { get; } = new List<Point>();

    public MovementGraph Graph { get; set; }

    private EnemyGroup _currentGroup;


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
        Graph = MovementGraph.BuildGraphFromMap(Map);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        ClearPaths(gameTime);
        SpawnEnemies(gameTime);
        UpdateGroups(gameTime);
    }

    private void ClearPaths(GameTime gameTime)
    {
        Graph.ClearPaths();
    }

    private void UpdateGroups(GameTime gameTime)
    {

        List<EnemyGroup> toRemove = new List<EnemyGroup>();
        foreach (EnemyGroup group in Groups)
        {
            group.Update(gameTime);
            if (group.IsDefeated)
            {
                toRemove.Add(group);
            }
        }

        foreach (EnemyGroup group in toRemove)
        {
            Groups.Remove(group);
        }
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

            var spawnTile = Map.MapTiles[SpawnLocations[_spawnLocationIndex].X, SpawnLocations[_spawnLocationIndex].Y];

            if (_spawnTiles.Count == 0)
            {
                _spawnTiles.Add(spawnTile);
            }

            _spawnLocationSet = false;
            _currentGroup = new EnemyGroup(spawnTile, this, new MoveToArtifactState());
            Groups.Add(_currentGroup);
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
            tile.Highlight(false);
        }
        _highlightedPath.Clear();
    }

    private void HightlightPath()
    {
        EnemyMove.Type actions = EnemyMove.Type.Run | EnemyMove.Type.Attack;
        MapTile startTile = Map.MapTiles[SpawnLocations[_spawnLocationIndex].X, SpawnLocations[_spawnLocationIndex].Y];

        List<EnemyMove> moves = Graph.GetPathToTile(startTile, Map.EnemyTarget, actions, 0);

        _highlightedPath.Add(startTile);

        foreach (EnemyMove nextMove in moves)
        {
            if (nextMove is RunMove runMove)
            {
                _highlightedPath.Add(runMove.Destination);
            }
        }

        foreach (var tile in _highlightedPath)
        {
            tile.Highlight(true);
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
            _currentGroup.Knights.Add(enemyObject.GetComponent<EnemyKnight>());
        }
        else if (_witchToSpawn > spawnTypeIndex - _knightToSpawn)
        {
            enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyWitch>(Config.ENEMY_WITCH_STATS, position, Quaternion.Identity);
            _witchToSpawn--;
            _currentGroup.Witches.Add(enemyObject.GetComponent<EnemyWitch>());
        }
        else if (_siegeToSpawn > spawnTypeIndex - (_knightToSpawn + _witchToSpawn))
        {
            enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyCatapult>(Config.ENEMY_CATAPULT_STATS, position, Quaternion.Identity);
            _siegeToSpawn--;
            _currentGroup.Catapults.Add(enemyObject.GetComponent<EnemyCatapult>());
        }
        else
        {
            return;
        }
    }

    //private void UpdatePathing(GameTime gameTime)
    //{
    //    Point start = Map.EnemyTarget;
    //    _graph.ComputeShortestPathToMapTile(start);
    //}

    internal float NextFloat()
    {
        return (float)rng.NextDouble();
    }
}
