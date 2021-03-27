using System;
using System.Collections.Generic;

public class Location
{
    private readonly List<EnemyMove> edges = new List<EnemyMove>();

    public void AddEdge(EnemyMove edge)
    {
        edges.Add(edge);
    }
}
