using System;
using System.Collections.Generic;
using System.Text;


class EnemyGroup
{
    MapTile SpawnPoint { get; }

    private readonly List<EnemyKnight> _knights = new List<EnemyKnight>();
    private readonly List<EnemyWitch> _witches = new List<EnemyWitch>();
    private readonly List<EnemyCatapult> _catapults = new List<EnemyCatapult>();

    public EnemyGroup(MapTile spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}

