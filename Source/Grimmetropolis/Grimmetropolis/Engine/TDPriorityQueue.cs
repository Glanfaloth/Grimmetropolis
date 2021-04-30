using System;
using System.Collections.Generic;
using System.Text;

class TDPriorityQueue<TValue>
{
    private List<Handle> _heap = new List<Handle>();

    public int Count => _heap.Count;

    public bool IsEmpty()
    {
        return _heap.Count == 0;
    }

    public Handle ExtractMin()
    {
        Handle result = _heap[0];

        Swap(0, _heap.Count - 1);
        _heap.RemoveAt(_heap.Count - 1);
        result._index = -1;
        SiftDown(0);

        return result;
    }

    public Handle Insert(float costPath, float costEstimation, TValue value)
    {
        Handle newHandle = new Handle(costPath, costEstimation, value, _heap.Count);

        _heap.Add(newHandle);
        SiftUp(newHandle._index);

        return newHandle;
    }

    public void DecreaseCostPath(Handle handle, float newCostPath, TValue value)
    {
        handle.CostPath = newCostPath;
        handle.Value = value;
        SiftUp(handle._index);
    }

    public void DecreaseCost(Handle handle, float newCostPath, float newCostEstimation, TValue value)
    {
        handle.CostPath = newCostPath;
        handle.CostEstimation = newCostEstimation;
        handle.Value = value;
        SiftUp(handle._index);
    }

    private void Swap(int a, int b)
    {
        Handle tmp = _heap[a];
        _heap[a] = _heap[b];
        _heap[b] = tmp;

        _heap[a]._index = a;
        _heap[b]._index = b;
    }

    private void SiftUp(int child)
    {
        int parent = (child - 1) / 2;
        while (child > 0 && _heap[parent].Cost > _heap[child].Cost)
        {
            Swap(parent, child);
            child = parent;
            parent = (child - 1) / 2;
        }
    }

    private void SiftDown(int index)
    {
        int left = index * 2 + 1;
        int right = left + 1;
        int smallest = index;

        if (left < _heap.Count && _heap[left].Cost < _heap[smallest].Cost)
        {
            smallest = left;
        }
        if (right < _heap.Count && _heap[right].Cost < _heap[smallest].Cost)
        {
            smallest = right;
        }
        if (smallest != index)
        {
            Swap(smallest, index);
            SiftDown(smallest);
        }
    }

    public class Handle
    {
        public float Cost => CostPath + CostEstimation;
        public TValue Value { get; internal set; }

        public float CostPath { get; internal set; }
        public float CostEstimation { get; internal set; }

        internal int _index;

        public Handle(float costPath, float costEstimation, TValue value, int index)
        {
            CostPath = costPath;
            CostEstimation = costEstimation;
            Value = value;
            _index = index;
        }


    }
}