using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{
    public void RespawnAll()
    {
        var spawners = FindObjectsByType<ObjectSpawner>(FindObjectsSortMode.None);
        
        foreach (var s in spawners)
        {
            s.EnsureSpawned();
        }
    }
}