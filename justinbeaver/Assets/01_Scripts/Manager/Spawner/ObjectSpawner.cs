using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [Header("스폰 타겟")]
    public GameObject ObjectPrefab;

    public bool respawnOnEnter = true;

    private GameObject spawned;

    private void OnEnable()
    {
        if (!respawnOnEnter)
        {
            return;
        }
        StartCoroutine(EnsureSpawnedNextFrame());
    }

    private IEnumerator EnsureSpawnedNextFrame()
    {
        yield return null;
        EnsureSpawned();
    }


    public void EnsureSpawned()
    {
        if (ObjectPrefab == null)
        {
            Debug.LogWarning($"[ObjectSpawner] ObjectPrefab이 비어있음: {name}");
            return;
        }


        if (ObjectManager.Instance == null)
        {
            Debug.LogError($"[ObjectSpawner] ObjectManager.Instance가 null임 (씬에 ObjectManager가 없거나 Awake 순서 문제): {name}");
            return;
        }


        // 이미 살아있으면 중복 스폰 방지
        if (spawned != null && spawned.activeInHierarchy)
        {
            return;
        }


        spawned = ObjectManager.Instance.SpawnObject(ObjectPrefab, transform.position, transform.rotation);
        if (spawned == null)
        {
            Debug.LogError($"[ObjectSpawner] SpawnObject가 null을 반환함. PoolManager/프리팹/매니저 상태 확인 필요: {name}");
            return;
        }


        var link = spawned.GetComponent<SpawnLink>();
        if (link == null)
        {
            link = spawned.AddComponent<SpawnLink>();
        }
        link.owner = this;
    }

    public void OnSpawnedDespawned()
    {
        spawned = null;
    }
}
