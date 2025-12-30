using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsContinue { get; private set; }

    public int SelectedMapIndex { get; private set; } = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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

        if (SoundManager.Instance != null) // BGM
        {
            SceneType type = SceneController.GetSceneType(scene.name);
            SoundManager.Instance.PlaySceneBGM(type);
        }

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
            if (RuleManager.Instance.IsTotalReset)
            {
                InitializeGame(); // 리셋시 새게임으로 강제하기
                StorageManager.Instance?.ImportSaveData(null);
                RuleManager.Instance.ClearTotalResetPoint();

                IsContinue = false;
                return;
            }

            if (IsContinue)
            {
                StartCoroutine(ApplyContinueDataWhenReady()); // 세이브체크
                IsContinue = false;
            }
            else
            {
                InitializeGame(); // 새게임 및 리스폰
            }
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

        controller.SetInputLocked(false);                                       // 플레이어 입력 잠금 해제
        controller.SetInventoryLocked(false);                                   // 인벤토리 잠금 해제
        controller.SetInventoryOpen(false);                                     // 인벤토리 오픈 해제

        context.playerStateMachine.ChangeState(new PlayerNormalState(context)); // 기본 상태로
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

    /// <summary>
    /// 이어하기 적용
    /// </summary>
    private IEnumerator ApplyContinueDataWhenReady()
    {      
        if (RuleManager.Instance.IsTotalReset) // 리셋 중이면 금지
        {
            Debug.Log("[Continue] Blocked due to TotalReset");
            yield break;
        }

        if (!SaveManager.HasSave())
            yield break;
      
        //while (ItemManager.Instance == null || StorageManager.Instance == null)
        //    yield return null;

        var data = SaveManager.Load();
        if (data == null)
            yield break;

        //Debug.Log("[Continue] Apply Save Data (Delayed)");

        HomeManager.Instance.SetLevel(data.houseLevel);
        RuleManager.Instance.SetFailCount(data.failCountAtcurrentLevel);
        StorageManager.Instance.ImportSaveData(data.storedItems);

        yield return null; // UI 바인딩 프레임 보장
    }

    public void SetContinue()
    {
        IsContinue = true;
    }

    /// <summary>
    /// 세이브
    /// </summary>
    public void SaveGame()
    {
        if (RuleManager.Instance == null ||
            StorageManager.Instance == null ||
            HomeManager.Instance == null)
            return;

        if (RuleManager.Instance.IsTotalReset)
            return; // 리셋 중에는 저장 금지

        var data = new SaveData
        {
            houseLevel = HomeManager.Instance.CurrentLevel,
            failCountAtcurrentLevel = RuleManager.Instance.GetEscapeFailCount(),
            storedItems = StorageManager.Instance.ExportSaveData()
        };

        SaveManager.Save(data);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[GameManager] Application Quit → Save");
        SaveGame();
    }
}
