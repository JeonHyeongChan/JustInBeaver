using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int preloadCount;
        public int maxSize;
    }

    public PoolConfig[] initialPools;
    private readonly Dictionary<int, ObjectPool<GameObject>> pools = new();
    private readonly Dictionary<int, Transform> parents = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;

        //초기 등록
        foreach (var cfg in initialPools)
        {
            if (cfg == null || cfg.prefab == null)
            {
                continue;
            }
            Register(cfg.prefab, cfg.preloadCount, cfg.maxSize);
        }
    }


    //프리팹 등록
    public void Register(GameObject prefab, int preloadCount = 0, int maxSize = 100)
    {
        int id = prefab.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            return;
        }

        var parent = new GameObject($"Pool_{prefab.name}").transform;
        parent.SetParent(transform);
        parents[id] = parent;

        var pool = new ObjectPool<GameObject>(
            createFunc: () => CreateNew(prefab, id, parent),
            actionOnGet: OnGet,
            actionOnRelease: (go) => OnRelease(go, parent),
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: preloadCount,
            maxSize: maxSize
        );


        for (int i = 0; i < preloadCount; i++)
        {
            var obj = pool.Get();
            pool.Release(obj);
        }
        pools[id] = pool;
    }

    //월드 스폰 (존재만 하면 되는 경우)
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        int id = prefab.GetInstanceID();
        var go = pools[id].Get();
        go.transform.SetPositionAndRotation(pos, rot);

        return go;
    }


    //컴포넌트 스폰 (조작하거나 움직이는 경우) 
    public T Spawn<T>(T prefabComponent, Vector3 pos, Quaternion rot)
    where T : Component
    {
        var prefabGO = prefabComponent.gameObject;
        int id = prefabGO.GetInstanceID();
        var go = pools[id].Get();
        go.transform.SetPositionAndRotation(pos, rot);

        return go.GetComponent<T>();
    }

    public void Despawn(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        var member = obj.GetComponent<PoolMember>();
        

        if (member == null || !pools.TryGetValue(member.prefabId, out var pool))
        {
            obj.SetActive(false);
            return;
        }
        pool.Release(obj);
    }


    private static GameObject CreateNew(GameObject prefab, int prefabId, Transform parent)
    {
        var go = Instantiate(prefab, parent);
        go.SetActive(false);


        var member = go.GetComponent<PoolMember>();

        if (member == null)
        {
            member = go.AddComponent<PoolMember>();
        }
        member.prefabId = prefabId;


        return go;
    }


    private static void OnGet(GameObject go)
    {
        go.SetActive(true);
        if (go.TryGetComponent<IPoolable>(out var pool))
        {
            pool.OnSpawned();
        }
    }


    private static void OnRelease(GameObject go, Transform parent)
    {
        if (go.TryGetComponent<IPoolable>(out var pool))
        {
            pool.OnDespawned();
        }
        go.SetActive(false);
        go.transform.SetParent(parent, false);
    }
}