using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject rootUI;

    private bool played;

    private void Awake()
    {
        if (rootUI) rootUI.SetActive(false);
    }

    public void PlayEnding()
    {
        Debug.Log("PlayEnding called");

        if (played)
        {
            return;
        }
        played = true;
        Time.timeScale = 0f;

        if (rootUI)
        {
            rootUI.SetActive(true);
        }

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is NULL");
            return;
        }
        Debug.Log("Preparing video...");
        videoPlayer.Stop();
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnFinished;
    }

    private void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("Prepared, playing video");
        vp.prepareCompleted -= OnPrepared;
        vp.Play();
    }

    private void OnFinished(VideoPlayer vp)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
