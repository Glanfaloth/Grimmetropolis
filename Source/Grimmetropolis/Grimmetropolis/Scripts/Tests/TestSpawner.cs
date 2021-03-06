using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TestSpawner : TDComponent
{
    private int _spawnLocationIndex = -1;
    private int _spawnTypeIndex = 0;

    private float _interval = .5f;
    private float _spawnTimer = 0f;
    private float _deleteTimer = 0f;

    private List<Enemy> enemies = new List<Enemy>();

    public List<Point> SpawnLocations { get; } = new List<Point>();

    public override void Initialize()
    {
        base.Initialize();

        _spawnTimer = _interval;
        _deleteTimer = 100f * _interval;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        _deleteTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_spawnTimer < 0f)
        {
            _spawnTimer += _interval;
            if (enemies.Count < 100)
            {
                _spawnLocationIndex = (_spawnLocationIndex + 1) % SpawnLocations.Count;
                _spawnTypeIndex++;

                Map map = GameManager.Instance.Map;
                Vector3 position = map.Corner + SpawnLocations[_spawnLocationIndex].ToVector3() + map.Offcenter;
                TDObject enemyObject;
                if (_spawnTypeIndex % 10 == 0)
                {
                    enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyCatapult>(Config.ENEMY_CATAPULT_STATS, position, Quaternion.Identity);
                }
                else if (_spawnTypeIndex % 3 == 0)
                {
                    enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyWitch>(Config.ENEMY_WITCH_STATS, position, Quaternion.Identity);
                }
                else
                {
                    enemyObject = PrefabFactory.CreateEnemyPrefab<EnemyKnight>(Config.ENEMY_KNIGHTS_STATS, position, Quaternion.Identity);
                }
                enemies.Add(enemyObject.GetComponent<Enemy>());
            }
        }

        if (_deleteTimer < 0f)
        {
            _deleteTimer += _interval;
            if (enemies.Count > 0)
            {
                Enemy enemy = enemies[0];
                enemies.RemoveAt(0);
                enemy.TDObject?.Destroy();
            }
        }
    }
}
