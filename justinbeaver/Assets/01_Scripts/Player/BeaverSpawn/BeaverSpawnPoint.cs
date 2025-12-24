using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeaverSpawnPoint : MonoBehaviour
{
    private static readonly Dictionary<string, BeaverSpawnPoint> SpawnPoints =
        new Dictionary<string, BeaverSpawnPoint>();

    private void OnEnable()
    {
        string sceneName = gameObject.scene.name;

        SpawnPoints[sceneName] = this;

        Debug.Log($"BeaverSpawnPoint for scene: {sceneName}");
    }

    private void OnDisable()
    {
        string sceneName = gameObject.scene.name;

        if (SpawnPoints.TryGetValue(sceneName, out var point) && point == this)
        {
            SpawnPoints.Remove(sceneName);
        }
    }

    public static BeaverSpawnPoint GetForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SpawnPoints.TryGetValue(sceneName, out var point);
        return point;
    }
}
