using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [Header("Map Prefabs")]
    public GameObject[] mapPrefabs; // 3개 배열

    void Start()
    {
        SpawnRandomMap();
    }

    void SpawnRandomMap()
    {
        if (mapPrefabs == null || mapPrefabs.Length == 0)
        {
            Debug.LogError("맵 프리팹이 설정되지 않음!");
            return;
        }

        int randomIndex = Random.Range(0, mapPrefabs.Length);
        Instantiate(mapPrefabs[randomIndex], Vector3.zero, Quaternion.identity);
    }
}