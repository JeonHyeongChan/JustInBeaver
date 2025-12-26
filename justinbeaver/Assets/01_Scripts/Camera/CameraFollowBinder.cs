using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowBinder : MonoBehaviour
{
    private CinemachineCamera cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Bind();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Bind();
    }

    private void Bind()
    {
        if (cam == null)
            return;

        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
            return;

        cam.Follow = player.transform;
        cam.LookAt = player.transform;
    }
}
