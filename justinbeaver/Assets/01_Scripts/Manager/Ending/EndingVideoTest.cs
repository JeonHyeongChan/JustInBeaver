using UnityEngine;
using UnityEngine.Video;

public class EndingVideoTest : MonoBehaviour
{
    private VideoPlayer vp;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        // F5 누르면 엔딩 영상 재생
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("[EndingTest] Play Video");
            Time.timeScale = 0f;
            vp.Play();
        }

        // ESC 누르면 정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[EndingTest] Stop Video");
            vp.Stop();
            Time.timeScale = 1f;
        }
    }
}
