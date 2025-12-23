using UnityEngine;

public class MapSpawner : MonoBehaviour
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

        PoolManager.Instance.Register(prefab, preloadCount: 0, maxSize: 4);

        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        currentMap = PoolManager.Instance.Spawn(prefab, pos, rot);        
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
