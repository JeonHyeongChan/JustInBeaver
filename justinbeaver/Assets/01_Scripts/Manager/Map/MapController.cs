using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapSet mapSet;
    public Transform spawnPoint;
    private GameObject currentMap;


    public void SpawnSelectedMap(int index)
    {
        DespawnCurrent();

        if (mapSet == null || mapSet.mapPrefabs == null || mapSet.mapPrefabs.Count == 0)
        {
            return;
        }

        index = Mathf.Clamp(index, 0, mapSet.mapPrefabs.Count - 1);
        var prefab = mapSet.mapPrefabs[index];

        PoolManager.Instance.Register(prefab, preloadCount: 0, maxSize: 1);
        currentMap = PoolManager.Instance.Spawn(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void DespawnCurrent()
    {
        if (currentMap != null)
        {
            PoolManager.Instance.Despawn(currentMap);
            currentMap = null;
        }
    }
}
