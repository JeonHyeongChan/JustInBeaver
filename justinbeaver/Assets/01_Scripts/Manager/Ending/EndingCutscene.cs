using UnityEngine;
using UnityEngine.Video;

public class EndingCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject rootUI;      //영상 UI(캔버스) 루트
    [SerializeField] private bool pauseGame = true;  //재생 중 게임 멈출지 여부
    [SerializeField] private bool lockPlayerInput = true;
    private float prevTimeScale = 1f;


    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponentInChildren<VideoPlayer>(true);
        }        
        if (rootUI != null)
        {
            rootUI.SetActive(false);
        }
    }


    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEndingCondition += PlayEnding;
        }
    }


    private void OnDisable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEndingCondition -= PlayEnding;
        }
    }


    private void PlayEnding()
    {
        if (videoPlayer == null)
        {
            return;
        }

        if (rootUI != null)
        {
            rootUI.SetActive(true);
        }

        //입력 잠금/일시정지
        if (lockPlayerInput)
        {
            var player = FindAnyObjectByType<PlayerController>();
            player?.SetInputLocked(true);
        }

        if (pauseGame)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }


    private void OnVideoFinished(VideoPlayer vp)
    {
        //재생 끝났을 때 처리
        if (pauseGame)
        {
            Time.timeScale = prevTimeScale;
        }

        if (lockPlayerInput)
        {
            var player = FindAnyObjectByType<PlayerController>();
            player?.SetInputLocked(false);
        }

        if (rootUI != null)
        {
            rootUI.SetActive(false);
        }
    }
}
