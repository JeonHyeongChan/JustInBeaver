using UnityEngine;

public class CaptureObject : MonoBehaviour, IPoolable
{
    [Header("갈무리 시간")]
    public float gatherDuration = 2f;

    [Header("해금 조건(거점 레벨)")]
    [Range(1, 5)] public int requiredHomeLevel = 1;

    [TextArea]
    public string lockedMessage = "The house level is low.";

    private DropTable dropTable;

    //오브젝트별 진행 상태
    public float progress { get; private set; }
    public bool isCompleted { get; private set; }

    private void Awake()
    {
        dropTable = GetComponent<DropTable>(); // 드랍아이템 참조
    }

    public void OnSpawned()
    {
        progress = 0f;
        isCompleted = false;
    }


    public void OnDespawned()
    {
        var link = GetComponent<SpawnLink>();

        if (link != null && link.owner != null)
        {
            link.owner.OnSpawnedDespawned();
        }
    }


    public bool IsUnlocked()
    {
        if (HomeManager.Instance == null)
        {
            return true;
        }
        return HomeManager.Instance.CurrentLevel >= requiredHomeLevel;
    }


    public string GetLockedMessage()
    {
        if (!string.IsNullOrEmpty(lockedMessage))
        {
            return lockedMessage;
        }
        return $"House level {requiredHomeLevel} need more";
    }


    public void SetProgress(float value01)
    {
        progress = Mathf.Clamp01(value01);
    }


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

        if (dropTable == null)
        {
            Debug.LogError($"[GatherComplete] DropTable missing on {name}");
            return;
        }

        dropTable.Drop(transform.position);

        //풀 반환
        ObjectManager.Instance.DropAndDespawn(gameObject);
    }
}
