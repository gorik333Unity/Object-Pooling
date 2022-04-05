using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField]
    private int _initialCapacity = 10;

    /// <summary>
    /// Pool that this poolable belongs to
    /// </summary>
    private Pool<Poolable> _pool;

    public int InitialCapacity { get => _initialCapacity; }

    /// <summary>
    /// Repool this instance, and move us under the poolmanager
    /// </summary>
    private void Repool()
    {
        transform.SetParent(PoolManager.Instance.transform, false);
        gameObject.SetActive(false);
        _pool.Return(this);
    }

    /// <summary>
    /// Set current pool
    /// </summary>
    /// <param name="pool">The pool that object belongs to</param>
    public void SetPool(Pool<Poolable> pool)
    {
        if (pool == null) throw new System.NullReferenceException();

        _pool = pool;
    }

    /// <summary>
    /// Check if pool doesn't null
    /// </summary>
    public bool IsPoolNull()
    {
        if (_pool != null)
            return false;
        else
            return true;
    }

    /// <summary>gameObject
    /// Pool the object if possible, otherwise destroy it
    /// </summary>
    /// <param name="gameObject">GameObject attempting to pool</param>
    public static void TryPool(GameObject gameObject)
    {
        var poolable = gameObject.GetComponent<Poolable>();

        if (poolable != null && !poolable.IsPoolNull() && PoolManager.instanceExists)
        {
            poolable.Repool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// If the prefab is poolable returns a pooled object otherwise instantiates a new object
    /// </summary>
    /// <param name="prefab">Prefab of object required</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>The pooled or instantiated component</returns>
    public static T TryGetPoolable<T>(GameObject prefab) where T : Component
    {
        var poolable = prefab.GetComponent<Poolable>();

        T instance = poolable != null && PoolManager.instanceExists ?
            PoolManager.Instance.GetPoolable(poolable).GetComponent<T>() : Instantiate(prefab).GetComponent<T>();

        return instance;
    }

    /// <summary>
    /// If the prefab is poolable returns a pooled object otherwise instantiates a new object
    /// </summary>
    /// <param name="prefab">Prefab of object required</param>
    /// <returns>The pooled or instantiated gameObject</returns>
    public static GameObject TryGetPoolable(GameObject prefab)
    {
        var poolable = prefab.GetComponent<Poolable>();
        GameObject instance = poolable != null && PoolManager.instanceExists ?
            PoolManager.Instance.GetPoolable(poolable).gameObject : Instantiate(prefab);

        return instance;
    }
}
