using System;
using System.Collections.Generic;

public class Location
{
    // for now make directed graph
    public List<EnemyMove> OutEdges { get; } = new List<EnemyMove>();
    public List<EnemyMove> InEdges { get; } = new List<EnemyMove>();
    public int Index { get; internal set; }

    internal void AddOutgoingEdge(EnemyMove edge)
    {
        OutEdges.Add(edge);
    }

    internal void AddIncomingEdge(EnemyMove edge)
    {
        InEdges.Add(edge);
    }

    internal void ClearIncomingEdges()
    {
        foreach (EnemyMove edge in InEdges)
        {
            edge.From.OutEdges.Remove(edge);
        }
        InEdges.Clear();
    }
    internal void ClearOutgoingEdges()
    {
        foreach (EnemyMove edge in OutEdges)
        {
            edge.To.InEdges.Remove(edge);
        }
        OutEdges.Clear();
    }
}
