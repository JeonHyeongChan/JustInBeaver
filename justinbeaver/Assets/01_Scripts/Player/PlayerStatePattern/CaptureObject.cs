using UnityEngine;

public class CaptureObject : MonoBehaviour, IPoolable
{
    [Header("갈무리 시간")]
    public float gatherDuration = 2f;

    [Header("드랍")]
    public DropTable dropTable;


    //오브젝트별 진행 상태
    public float progress { get; private set; }     //오브젝트별 진행도
    public bool isCompleted { get; private set; }   //완료(소모) 여부


    private void Awake()
    {
        dropTable = GetComponent<DropTable>();
    }


    //풀에서 꺼내질 때 초기화(리스폰/재배치 시)
    public void OnSpawned()
    {
        progress = 0f;
        isCompleted = false;
    }

    public void OnDespawned() { }

    public void SetProgress(float value01)
    {
        progress = Mathf.Clamp01(value01);
    }

    //갈무리 중일 때만 호출
    public void TickGather(float dt)
    {
        if (isCompleted)
        {
            return;
        }
        float dur = Mathf.Max(0.05f, gatherDuration);
        progress = Mathf.Clamp01(progress + dt / dur);
    }


    public void OnGatherComplete()
    {
        if (isCompleted)
        {
            return;
        }
        isCompleted = true;


        //드랍
        if (dropTable != null && dropTable.dropItem != null)
        {
            int count = Random.Range(dropTable.minCount, dropTable.maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                Vector3 p = transform.position + Vector3.up * 0.5f;
                ItemManager.Instance.SpawnItem(dropTable.dropItem, p, Quaternion.identity);
            }
        }

        //풀 반환
        PoolManager.Instance.Despawn(gameObject);
    }
}
