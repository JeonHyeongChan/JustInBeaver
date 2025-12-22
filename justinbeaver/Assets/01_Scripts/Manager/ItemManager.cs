using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public ItemDatabase database;

    [Header("Drop")]
    [SerializeField] private LayerMask groundMask = ~0;
    [HideInInspector] private float groundRayHeight = 0f;
    [HideInInspector] private float groundRayDistance = 0.1f;
    [HideInInspector] private float groundOffsetY = 0.01f;
    [HideInInspector] private float groundProbeRadius = 0.25f;


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


        //바닥에 붙게 추가 보정
        SnapSpawnedPickupToGround(go, 0.01f);


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

        if (Physics.SphereCast(origin, groundProbeRadius, Vector3.down, out RaycastHit hit,
                groundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            pos.y = hit.point.y + groundOffsetY;
        }
        else
        {
            //못 맞추면 최소한 과하게 뜨지 않도록 살짝 내림
            pos.y = pos.y - 0.5f;
        }
        return pos;
    }



    private void AlignToGroundByBounds(GameObject go, float extraOffset = 0.01f)
    {
        if (go == null)
        {
            return;
        }    
            
        //콜라이더가 있으면 콜라이더 바운드 사용
        var col = go.GetComponentInChildren<Collider>();
        if (col != null)
        {
            float bottom = col.bounds.min.y;
            float delta = (go.transform.position.y - bottom) + extraOffset;
            go.transform.position -= new Vector3(0f, delta, 0f);
            return;
        }

        //없으면 Renderer 바운드 사용
        var rend = go.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            float bottom = rend.bounds.min.y;
            float delta = (go.transform.position.y - bottom) + extraOffset;
            go.transform.position -= new Vector3(0f, delta, 0f);
        }
    }

    private void SnapSpawnedPickupToGround(GameObject go, float extraOffset = 0.01f)
    {
        if (go == null) return;

        var col = go.GetComponentInChildren<Collider>();
        if (col == null)
        {
            AlignToGroundByBounds(go, extraOffset);
            return;
        }

        float radius = Mathf.Max(col.bounds.extents.x, col.bounds.extents.z);
        radius = Mathf.Max(radius, 0.05f);

        Vector3 origin = col.bounds.center + Vector3.up * groundRayHeight;

        if (Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit,
                groundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            float bottom = col.bounds.min.y;
            float deltaToBottom = go.transform.position.y - bottom;

            Vector3 p = go.transform.position;
            p.y = hit.point.y + deltaToBottom + extraOffset;
            go.transform.position = p;
        }
        else
        {
            AlignToGroundByBounds(go, extraOffset);
        }
    }

}

