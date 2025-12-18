using UnityEngine;

public class CaptureObject : MonoBehaviour, IPoolable
{
    [Header("갈무리 시간")]
    public float gatherDuration = 2f;

    //오브젝트별 진행 상태
    public float progress { get; private set; }
    public bool isCompleted { get; private set; }

    private void Awake() { }

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

    public void SetProgress(float value01)
    {
        progress = Mathf.Clamp01(value01);
    }

    public void TickGather(float dt)
    {
        if (isCompleted) return;

        float dur = Mathf.Max(0.05f, gatherDuration);
        progress = Mathf.Clamp01(progress + dt / dur);
    }

    public void OnGatherComplete()
    {
        if (isCompleted) return;
        isCompleted = true;

        //풀 반환
        ObjectManager.Instance.DropAndDespawn(gameObject);
    }
}
