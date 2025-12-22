using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public ItemDatabase database;

    [Header("Drop")]
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundRayHeight = 2f;
    [SerializeField] private float groundRayDistance = 10f;
    [SerializeField] private float groundOffsetY = 0.02f; //바닥에 박힘 방지


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


    /// <summary>
    /// 인벤토리에서 버릴 때: itemId로 월드(필드)에 아이템 픽업을 생성.
    /// - 바닥에 스냅
    /// - 풀에서 스폰
    /// - ItemPickup.itemId 주입
    /// </summary>
    public GameObject SpawnPickupByItemId(string itemId, Vector3 pos, Quaternion rot)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }

        var data = GetItem(itemId);

        if (data == null)
        {
            return null;
        }

        if (data.itemPrefab == null)
        {
            return null;
        }

        //바닥 스냅
        pos = SnapToGround(pos);

        //풀 스폰
        var go = SpawnItem(data, pos, rot);

        if (go == null)
        {
            return null;
        }

        //스폰된 픽업에 itemId 주입
        var pickup = go.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            pickup.itemId = itemId;
        }
        return go;
    }


    //오버로드
    public GameObject SpawnPickupByItemId(string itemId, Vector3 pos)
    {
        return SpawnPickupByItemId(itemId, pos, Quaternion.identity);
    }


    private Vector3 SnapToGround(Vector3 pos)
    {
        Vector3 origin = pos + Vector3.up * groundRayHeight;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit,
                groundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            pos.y = hit.point.y + groundOffsetY;
        }

        return pos;
    }
}

