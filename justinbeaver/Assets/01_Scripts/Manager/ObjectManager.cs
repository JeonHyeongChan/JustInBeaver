using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }
    private readonly HashSet<GameObject> active = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public GameObject SpawnObject(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null)
        {
            return null;
        }    
            

        PoolManager.Instance.Register(prefab);
        var obj = PoolManager.Instance.Spawn(prefab, pos, rot);
        if (obj != null)
        {
            active.Add(obj);
        }
        return obj;
    }


    public void DespawnObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        active.Remove(obj);
        PoolManager.Instance.Despawn(obj);
    }


    //오브젝트 파괴/갈무리 완료 후 호출 (드랍 후 반환)
    public void DropAndDespawn(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        var drop = obj.GetComponent<DropTable>();
        
        if (drop != null && drop.dropItem != null)
        {
            int count = Random.Range(drop.minCount, drop.maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                Vector3 p = obj.transform.position + Vector3.up * 0.5f;
                ItemManager.Instance.SpawnItem(drop.dropItem, p, Quaternion.identity);
            }
        }
        DespawnObject(obj);
    }
}
