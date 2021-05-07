using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


public class EnemyGroup
{

    public List<EnemyKnight> Knights { get; } = new List<EnemyKnight>();
    public List<EnemyWitch> Witches { get; } = new List<EnemyWitch>();
    public List<EnemyCatapult> Catapults { get; } = new List<EnemyCatapult>();
    public bool IsDefeated => (Knights.Count + Witches.Count + Catapults.Count) == 0;

    public Enemy Leader { get; set; }
    public Enemy ArtifactBearer { get; set; }

    public MapTile SpawnPoint { get; set; }

    public MovementGraph Graph => _controller.Graph;

    private readonly EnemyController _controller;

    private EnemyGroupState _state;

    public IEnumerable<Enemy> AllEnemies => Knights.Concat<Enemy>(Witches).Concat(Catapults);

    public EnemyGroup(MapTile spawnPoint, EnemyController controller, EnemyGroupState startState)
    {
        SpawnPoint = spawnPoint;
        _controller = controller;
        _state = startState;
    }

    internal void Update(GameTime gameTime)
    {
        RemoveDeadEnemies(Knights);
        RemoveDeadEnemies(Witches);
        RemoveDeadEnemies(Catapults);

        if (!IsDefeated)
        {
            Leader = GetCurrentLeader();
            ArtifactBearer = GetArtifactBearer();
        }

        _state.SendCommands(this);
        _state = _state.UpdateState(this);
    }

    private Enemy GetArtifactBearer()
    {
        if (ArtifactBearer != null && ArtifactBearer.Health > 0)
            return ArtifactBearer;

        foreach (var enemy in AllEnemies)
        {
            if(enemy.Items[0] is MagicalArtifact)
            {
                return enemy;
            }
        }

        return null;
    }

    private Enemy GetCurrentLeader()
    {
        if (Knights.Count > 0)
        {
            return Knights[0];
        }
        if (Witches.Count > 0)
        {
            return Witches[0];
        }
        if (Catapults.Count > 0)
        {
            return Catapults[0];
        }

        return null;
    }

    private void RemoveDeadEnemies<T>(List<T> enemies) where T : Enemy
    {
        int index = enemies.Count - 1;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[index].Health <= 0)
            {
                enemies.RemoveAt(index);
            }
        }
    }
}

