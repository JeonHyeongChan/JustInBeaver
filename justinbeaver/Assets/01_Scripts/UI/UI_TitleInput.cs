using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TitleInput : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button exitGameButton;

    private Button[] buttons;
    private int currentIndex = 0;

    private void Start()
    {
        buttons = new Button[] { newGameButton, exitGameButton };

        SelectButton(0);
    }

    private void Update()
    {
        HandleNavigation();
        HandleSubmit();
    }

    /// <summary>
    /// 현재 선택한 인덱스(버튼) 변경
    /// </summary>
    /// <param name="direction"></param>
    private void MoveSelection(int direction)
    {
        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = buttons.Length - 1;
        else if (currentIndex >= buttons.Length)
            currentIndex = 0;

        SelectButton(currentIndex);
    }

    /// <summary>
    /// 선택한 버튼의 이벤트처리
    /// </summary>
    /// <param name="index"></param>
    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    /// <summary>
    /// 버튼 이동
    /// </summary>
    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))        // 0
            MoveSelection(1);                           // ▼
        else if (Input.GetKeyDown(KeyCode.UpArrow))     // 1
            MoveSelection(-1);
    }

    /// <summary>
    /// 버튼 확인처리
    /// </summary>
    private void HandleSubmit()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentIndex == 0)
                OnNewGame();
            else if (currentIndex == 1)
                OnExitGame();
        }
    }

    private void OnNewGame()
    {
        SceneController.Instance.LoadScene(SceneType.BeaverHouse); //씬 전환
    }

    private void OnExitGame()
    {
        Debug.Log("Exit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
