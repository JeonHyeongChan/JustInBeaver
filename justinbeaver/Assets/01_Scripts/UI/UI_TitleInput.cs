using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_TitleInput : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitGameButton;

    private Button[] buttons;
    private int currentIndex = 0;

    private void Start()
    {
        //buttons = new Button[] { newGameButton, exitGameButton };
        BuildButtons();
        SelectButton(0);
    }

    private void Update()
    {
        HandleNavigation();
        HandleSubmit();
    }

    private void BuildButtons()
    {
        var list = new List<Button>();

        bool hasSave = SaveManager.HasSave();

        if (continueButton != null)
            continueButton.gameObject.SetActive(hasSave);

        if (hasSave && continueButton != null)
            list.Add(continueButton);

        if (newGameButton != null)
            list.Add(newGameButton);

        if (exitGameButton != null)
            list.Add(exitGameButton);

        buttons = list.ToArray();
        currentIndex = 0;
    }

    /// <summary>
    /// 현재 선택한 인덱스(버튼) 변경
    /// </summary>
    /// <param name="direction"></param>
    private void MoveSelection(int direction)
    {
        if (buttons == null || buttons.Length == 0)
            return;

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
        if (buttons == null || index < 0 || index >= buttons.Length)
            return;

        EventSystem.current.SetSelectedGameObject(null);
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
        if (!Input.GetKeyDown(KeyCode.Z))
            return;

        if (buttons == null || buttons.Length == 0)
            return;

        SoundManager.Instance?.PlaySFX(SFXType.ButtonClick); // 사운드

        var selected = buttons[currentIndex];

        if (selected == continueButton)
            OnContinue();
        else if (selected == newGameButton)
            OnNewGame();
        else if (selected == exitGameButton)
            OnExitGame();

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    SoundManager.Instance?.PlaySFX(SFXType.ButtonClick); // 사운드
        //
        //    if (currentIndex == 0)
        //        OnNewGame();
        //    else if (currentIndex == 1)
        //        OnExitGame();
        //}
    }

    /// <summary>
    /// 새 게임
    /// </summary>
    private void OnNewGame()
    {
        SaveManager.Delete();

        StorageManager.Instance?.ImportSaveData(null);
        HomeManager.Instance?.SetLevel(1);
        RuleManager.Instance?.SetFailCount(0);

        SceneController.Instance.LoadScene(SceneType.BeaverHouse); //씬 전환
    }

    /// <summary>
    /// 이어하기
    /// </summary>
    private void OnContinue()
    {
        if (!SaveManager.HasSave())
            return;

        GameManager.Instance.SetContinue();
        SceneController.Instance.LoadScene(SceneType.BeaverHouse);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
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
