using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    private List<Poolable> _poolable;
    private Dictionary<Poolable, Pool<Poolable>> m_pool;

    public void OnStartGame()
    {
        SpawnPool();
    }

    public void Return(Poolable poolable)
    {
        Pool<Poolable> pool = m_pool[poolable];

        pool.Return(poolable);
    }

    public Poolable GetPoolable(Poolable poolable)
    {
        if (!m_pool.ContainsKey(poolable))
        {
            m_pool.Add(poolable, new Pool<Poolable>(poolable.gameObject, poolable.InitialCapacity, Initialize));
        }

        Pool<Poolable> pool = m_pool[poolable];
        Poolable spawnedInstance = pool.Get();

        spawnedInstance.SetPool(pool);

        return spawnedInstance;
    }

    private void Start()
    {
        OnStartGame();
    }

    private void Initialize(Component poolable)
    {
        poolable.transform.SetParent(transform, false);
        poolable.gameObject.SetActive(false);
    }

    private void SpawnPool()
    {
        m_pool = new Dictionary<Poolable, Pool<Poolable>>();

        foreach (var poolable in _poolable)
        {
            if (poolable == null) continue;

            m_pool.Add(poolable, new Pool<Poolable>(poolable.gameObject, poolable.InitialCapacity, Initialize));
        }
    }
}
