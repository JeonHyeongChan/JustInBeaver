using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Pause : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject helpPanel;

    [Header("First Selected Buttons")]
    [SerializeField] private Button firstMainButton;
    [SerializeField] private Button firstOptionsButton;
    [SerializeField] private Button firstHelpButton;

    [Header("Quit")]
    [SerializeField] private string titleSnene = "TitleScene";


    private void OnEnable()
    {
        ShowMain();
    }


    //재개
    public void OnClickResume()
    {
        UIManager.Instance?.HidePauseUI();
    }


    //옵션
    public void OnClickOption()
    {
        SetPanels(main: false, options: true, help: false);
        Select(firstOptionsButton);
    }


    //도움말
    public void OnClickHelp()
    {
        SetPanels(main: false, options: false, help: true);
        Select(firstHelpButton);
    }


    //종료
    public void OnClickQuit()
    {
        UIManager.Instance?.HidePauseUI();

        if (!string.IsNullOrEmpty(titleSnene))
        {
            SceneManager.LoadScene(titleSnene);
        }   
        else
        {
            Debug.Log("Exit Game");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    public void OnClickBack()
    {
        ShowMain();
    }


    private void ShowMain()
    {
        SetPanels(main: true, options: false, help: false);
        Select(firstMainButton);
    }

    private void SetPanels(bool main, bool options, bool help)
    {
        Debug.Log($"[UI_Pause] SetPanels main={main} options={options} help={help} | " +
                  $"mainPanel={(mainPanel ? mainPanel.name : "NULL")} helpPanel={(helpPanel ? helpPanel.name : "NULL")}");

        if (mainPanel != null) mainPanel.SetActive(main);
        if (optionsPanel != null) optionsPanel.SetActive(options);
        if (helpPanel != null) helpPanel.SetActive(help);
    }

    private void Select(Button btn)
    {
        if (btn == null)
        {
            return;
        }
        if (EventSystem.current == null)
        {
            return;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btn.gameObject);
    }

    public bool IsInSubPanel
    {
        get
        {
            if (optionsPanel != null && optionsPanel.activeSelf)
            {
                return true;
            }   
            if (helpPanel != null && helpPanel.activeSelf)
            {
                return true;
            }
            return false;
        }
    }

}
