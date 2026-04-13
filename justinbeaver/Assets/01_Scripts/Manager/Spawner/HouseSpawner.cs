using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [Header("House Prefabs")]
    [SerializeField] private GameObject[] housePrefabs;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;

    private GameObject currentHouse;

    private void Start()
    {
        if (HomeManager.Instance == null)
            return;

        HomeManager.Instance.OnHouseLevelChanged += SpawnHouseByLevel;  // 이벤트

        SpawnHouseByLevel(HomeManager.Instance.CurrentLevel);   // 1레벨부터
    }

    private void OnDestroy()
    {
        if (HomeManager.Instance != null)
        {
            HomeManager.Instance.OnHouseLevelChanged -= SpawnHouseByLevel;
        }
    }

    private void SpawnHouseByLevel(int level)
    {
        int index = level - 1;

        if (index < 0 || index >= housePrefabs.Length)
        {
            Debug.LogError($"House prefabs not found {level}");
            return;
        }

        if (currentHouse != null)
        {
            Destroy(currentHouse);
        }

        currentHouse = Instantiate(housePrefabs[index], 
            spawnPoint.position, spawnPoint.rotation, transform);

        Debug.Log($"Spawned House {level}");

        MoveBeaverToHouseSpawn(currentHouse);
    }

    private void MoveBeaverToHouseSpawn(GameObject house)
    {
        if (Beaver.Instance == null)
            return;

        //새 하우스 프리펩 안에서 SpawnPoint 검색
        var spawn = house.GetComponentInChildren<BeaverSpawnPoint>();

        if (spawn == null)
        {
            Debug.LogWarning("BeaverSpawnPoint not found in house prefab");
            return;
        }

        Beaver.Instance.transform.position = spawn.transform.position;
        Beaver.Instance.transform.rotation = spawn.transform.rotation;       
    }
}
