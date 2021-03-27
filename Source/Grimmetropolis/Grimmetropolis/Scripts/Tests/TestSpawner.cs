using Microsoft.Xna.Framework;

using System.Collections.Generic;

public class TestSpawner : TDComponent
{
    private float _interval;
    private float _spawnTimer;
    private float _deleteTimer;

    private List<Enemy> enemies = new List<Enemy>();

    public override void Initialize()
    {
        base.Initialize();

        _interval = 4f;
        _spawnTimer = _interval;
        _deleteTimer = 5f * _interval;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        _deleteTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_spawnTimer < 0f)
        {
            _spawnTimer += _interval;
            TDObject enemyObject = PrefabFactory.CreatePrefab(PrefabType.Enemy, new Vector3(-8f, 0f, 0f), Quaternion.Identity);
            enemies.Add(enemyObject.GetComponent<Enemy>());
        }

        if (_deleteTimer < 0f)
        {
            _deleteTimer += _interval;
            Enemy enemy = enemies[0];
            enemies.RemoveAt(0);
            enemy.TDObject.Destroy();
        }
    }
}
