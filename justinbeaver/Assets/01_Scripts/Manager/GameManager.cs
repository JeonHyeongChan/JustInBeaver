using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int SelectedMapIndex { get; private set; } = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        //인간 집 랜덤 선택
        if (scene.name == "HumanHouseScene")
        {
            var spawner = FindAnyObjectByType<MapSpawner>();
            if (spawner != null && spawner.mapSet != null)
            {
                int count = spawner.mapSet.mapPrefabs.Count;
                if (count <= 0)
                {
                    return;
                }
                    
                //매번 랜덤 재선택
                SelectedMapIndex = Random.Range(0, count);

                spawner.SpawnSelectedMap(SelectedMapIndex);
            }
        }

        //게임 초기화
        if (scene.name == "BeaverHouseScene")
        {
            InitializeGame();
        }

        //스폰 위치로 비버 이동
        StartCoroutine(MoveBeaverReady());
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
        var controller = context.playerController;

        health.ResetHealth();                                                   // 체력 초기화

        rigid.isKinematic = false;                                              // 물리 복구
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        animator.Rebind();                                                      // 애니메이터 초기화
        animator.Update(0f);
        animator.ResetTrigger("Die");

        context.playerController.enabled = true;                                // 컨트롤러 활성화
        context.playerStateMachine.ChangeState(new PlayerNormalState(context)); // 기본 상태로

        controller.SetInputLocked(false);                                       // 플레이어 입력 잠금 해제        
    }

    /// <summary>
    /// 비버 리스폰
    /// </summary>
    public void HandlePlayerRespawn()
    {
        Debug.Log("Beaver Respanwn");

        SceneController.Instance.LoadScene(SceneType.BeaverHouse);  
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


    public void SelectRandomMap(int mapCount)
    {
        SelectedMapIndex = Random.Range(0, mapCount);
    }


    public void GoToField(MapSet set)
    {
        if (set == null || set.mapPrefabs.Count == 0)
        {
            return;
        }
        SelectRandomMap(set.mapPrefabs.Count);        
        SceneController.Instance.LoadScene(SceneType.HumanHouse);
    }

    /// <summary>
    /// 비버 스폰위치로 지연이동
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveBeaverReady()
    {
        yield return null;

        var spawnPoint = BeaverSpawnPoint.GetForCurrentScene();

        if (spawnPoint == null)
        {            
            yield break;
        }

        if (Beaver.Instance == null)
            yield break;

        Beaver.Instance.transform.position = spawnPoint.transform.position;
        Beaver.Instance.transform.rotation = spawnPoint.transform.rotation;

        Debug.Log($"{spawnPoint.gameObject.scene.name}");
    }
}
