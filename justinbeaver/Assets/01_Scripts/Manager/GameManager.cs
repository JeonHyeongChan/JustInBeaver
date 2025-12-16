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

    private void OnDestroy()
    {        
        SceneManager.sceneLoaded -= OnSceneLoaded; // 메모리 누수 방지
    }

    //씬 로드 완료시 게임 필수요소 생성
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        bool isTitle = scene.name == "TitleScene";

        Beaver.Instance.gameObject.SetActive(!isTitle); // TitleScene에서는 비활성화 (추락으로 인한 물리연산 적용 안되게)

        //비버 Spawn 처리
        if (!isTitle && BeaverSpawnPoint.Current != null)
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

        //비버 초기화
        //player.player.InitializePlayer();


        //맵 초기화 (인간포함)
        //spawner.InitializeSpawner();
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
