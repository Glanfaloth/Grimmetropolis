using System;
using System.Collections.Generic;
using System.Text;

class TDPriorityQueue<TKey, TValue>
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

    public Handle Insert(float cost, TKey key, TValue value)
    {
        Handle newHandle = new Handle(cost, key, value, _heap.Count);

        _heap.Add(newHandle);
        SiftUp(newHandle._index);

        return newHandle;
    }

    public Handle UdpateItem(Handle handle, TValue value, float newCost)
    {
        throw new NotImplementedException();
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
        public float Cost { get; internal set; }
        public TKey Key { get; internal set; }
        public TValue Value { get; internal set; }

        internal int _index;

        public Handle(float cost, TKey key, TValue value, int index)
        {
            Cost = cost;
            Key = key;
            Value = value;
            _index = index;
        }


    }
}