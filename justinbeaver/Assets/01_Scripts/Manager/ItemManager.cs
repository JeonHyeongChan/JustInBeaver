using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public ItemDatabase database;


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

    public ItemData GetItem(string itemId)
    {
        return database != null ? database.Find(itemId) : null;
    }


    public GameObject SpawnItem(ItemData item, Vector3 pos, Quaternion rot)
    {
        if (item == null || item.itemPrefab == null)
        {
            return null;
        }
        PoolManager.Instance.Register(item.itemPrefab);
        return PoolManager.Instance.Spawn(item.itemPrefab, pos, rot);
    }
}

