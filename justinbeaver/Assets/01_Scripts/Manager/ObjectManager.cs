using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }
    private readonly HashSet<GameObject> active = new();

    [HideInInspector] private float dropRayStartHeight = 0f;   //레이 시작 높이
    [HideInInspector] private float dropRayDistance = 0.1f;     //레이 길이
    [HideInInspector] private float dropGroundOffset = 0f;  //바닥에 박힘 방지 오프셋
    [SerializeField] private LayerMask groundMask;



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

            //레이를 오브젝트 위에서 아래로 쏴서 바닥 위치를 찾음
            Vector3 rayOrigin = obj.transform.position + Vector3.up * dropRayStartHeight;
            Vector3 baseDropPos = obj.transform.position;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, dropRayDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                baseDropPos = hit.point + Vector3.up * dropGroundOffset;
            }
            else
            {
                //바닥을 못 찾으면 최소한 지면 쪽으로 과도하게 뜨지 않게
                baseDropPos = obj.transform.position;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = baseDropPos;

                ItemManager.Instance.SpawnItem(drop.dropItem, spawnPos, Quaternion.identity);
            }
        }
        DespawnObject(obj);
    }
}
