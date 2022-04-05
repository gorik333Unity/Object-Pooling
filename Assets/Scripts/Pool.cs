using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class Pool<T>
{
    private List<Poolable> _allItem;
    private List<Poolable> _available;
    private Action<Component> _initialize;
    private GameObject _prefab;

    public Pool(GameObject prefab, int initialCapacity, Action<Component> initialize)
    {
        _allItem = new List<Poolable>();
        _available = new List<Poolable>();

        _prefab = prefab;

        _initialize = initialize;

        PrefabFactory(prefab, initialCapacity);
    }

    public Poolable Get()
    {
        if (_available.Count == 0)
            AddNewElements(_prefab, 1);

        var used = _available[0];

        _available.Remove(used);

        return used;
    }

    public void Return(Poolable item)
    {
        if (!_available.Contains(item))
            _available.Add(item);
    }

    public void AddNewElements(GameObject prefab, int amount)
    {
        if (amount < 1) return;

        for (int i = 0; i < amount; i++)
        {
            var obj = Object.Instantiate(prefab).GetComponent<Poolable>();

            _available.Add(obj);
            _allItem.Add(obj);

            _initialize(obj);
        }
    }

    private void PrefabFactory(GameObject prefab, int initialCapacity)
    {
        if (initialCapacity < 1) return;

        AddNewElements(prefab, initialCapacity);
    }
}
