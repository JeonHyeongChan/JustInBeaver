using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnPlayerRespawnRequired += HandlePlayerRespawn;
        }
    }

    private void OnDestroy()
    {        
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnPlayerRespawnRequired -= HandlePlayerRespawn;
        }
    }

    //씬 로드 완료시 게임 필수요소 생성
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        bool isTitle = scene.name == "TitleScene";

        if (Beaver.Instance != null)
        {
            Beaver.Instance.gameObject.SetActive(!isTitle); // TitleScene에서는 비활성화
        }

        //비버 Spawn 처리
        if (!isTitle && BeaverSpawnPoint.Current != null && Beaver.Instance != null)
        {
            Beaver.Instance.transform.position = BeaverSpawnPoint.Current.position;
        }

        //게임 초기화
        if (scene.name == "BeaverHouseScene")
        {
            InitializeGame();
        }        
    }

    private void InitializeGame()
    {
        Debug.Log("Game Intialiezed");

        if (Beaver.Instance == null)
        {
            Debug.LogError("Beaver is null");
            return;
        }

        //비버 초기화
        var context = Beaver.Instance.GetComponent<PlayerContext>();
        var health = Beaver.Instance.GetComponent<PlayerHealth>();
        var rigid = Beaver.Instance.GetComponent<Rigidbody>();
        var animator = context.playerAnimator;

        health.ResetHealth();                                                   // 체력 초기화

        rigid.isKinematic = false;                                              // 물리 복구
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        animator.Rebind();                                                      // 애니메이터 초기화
        animator.Update(0f);
        animator.ResetTrigger("Die");

        context.playerController.enabled = true;                                // 컨트롤러 활성화
        context.playerStateMachine.ChangeState(new PlayerNormalState(context)); // 기본 상태로

        //맵 초기화 (인간포함)
        //spawner.InitializeSpawner();
    }

    public void HandlePlayerRespawn()
    {
        Debug.Log("Beaver Respanwn");

        SceneController.Instance.LoadScene(SceneType.BeaverHouse);  // 비버집으로
    }

    public void EndingAfterReturnToTitle()
    {
        Debug.Log("Ending and To Title");

        CleanGameState();

        UIManager.Instance.HideAllUI();

        SceneController.Instance.LoadScene(SceneType.Title);
    }

    private void CleanGameState()
    {
        //RuleManager 리셋
        //플레이어 상태 저장
        //등등
    }
}
