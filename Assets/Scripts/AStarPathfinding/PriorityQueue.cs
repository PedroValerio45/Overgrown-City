using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PriorityQueue<T>
{
    private readonly SortedDictionary<float, Queue<T>> _elements = new SortedDictionary<float, Queue<T>>();
    private readonly HashSet<T> _contained = new HashSet<T>();
    public int Count { get; private set; } = 0;

    public void Enqueue(T item, float priority)
    {
        if (!_elements.TryGetValue(priority, out var queue))
        {
            queue = new Queue<T>();
            _elements[priority] = queue;
        }
        queue.Enqueue(item);
        _contained.Add(item);
        Count++;
    }

    public T Dequeue()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        var firstPair = _elements.First();
        var item = firstPair.Value.Dequeue();
        if (firstPair.Value.Count == 0)
            _elements.Remove(firstPair.Key);

        _contained.Remove(item);
        Count--;
        return item;
    }

    public bool Contains(T item)
    {
        return _contained.Contains(item);
    }

    public void Clear()
    {
        _elements.Clear();
        _contained.Clear();
        Count = 0;
    }
}
