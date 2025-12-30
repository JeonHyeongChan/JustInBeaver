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
        ResetState();
    }

    private void ResetState() // 재생 조건 정리
    {
        played = false;

        if (rootUI)
            rootUI.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.enabled = false;
        }
    }

    public void PlayEnding()
    {
        Debug.Log("PlayEnding called");

        if (played)
        {
            return;
        }

        played = true;
        SoundManager.Instance?.MuteBGM(true);

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

        if (!videoPlayer.enabled)
        {
            Debug.LogWarning("VideoPlayer was disabled -> enabling it now");
            videoPlayer.enabled = true;
        }

        if (!videoPlayer.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("VideoPlayer GameObject was inactive -> activating it now");
            videoPlayer.gameObject.SetActive(true);
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
        SoundManager.Instance?.MuteBGM(false);

        ResetState();

        SceneManager.LoadScene("TitleScene");
    }      
}
