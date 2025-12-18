using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("InGame")]
    public GameObject hudUI;
    public GameObject inventoryUI;
    public GameObject gatherGaugeUI;
    public GameObject escapeResultUI;

    public UI_GatherGauge gatherGauge;
    public UI_InteractHint interactHint;

    [Header("OutGame")]
    public GameObject shopUI;
    public GameObject upgradeUI;

    [Header("Common")]
    public GameObject gameOverUI;
    public GameObject victoryUI;

    public UI_GatherGauge GatherGauge => gatherGauge;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            HideAllUI();
        }
        else
        {
            Destroy(gameObject);    // 싱글톤으로 수정
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindSceneUI();
    }

    /// <summary>
    /// 씬 전환 시 모든 씬 UI 재바인딩
    /// </summary>
    private void BindSceneUI()
    {
        BindGatherGauge();
        BindInteractHint();
    }

    private void BindGatherGauge()
    {
        gatherGauge = FindAnyObjectByType<UI_GatherGauge>(
            FindObjectsInactive.Include);

        if (gatherGauge == null)
        {
            Debug.Log("GatherGauge Not Found in this Scene");
            gatherGaugeUI = null;
            return;
        }

        gatherGaugeUI = gatherGauge.gameObject;
        gatherGaugeUI.SetActive(false);
    }

    private void BindInteractHint()
    {
        interactHint = FindAnyObjectByType<UI_InteractHint>(
            FindObjectsInactive.Include);

        if (interactHint == null)
        {
            Debug.Log("InteractHint Not Found in this Scene");
            return;
        }

        interactHint.Hide();
    }

    /// <summary>
    /// 갈무리 UI
    /// </summary>
    public void ShowGatherGauge()
    {
        if (gatherGaugeUI)
            gatherGaugeUI.SetActive(true);
    }

    public void HideGahterGauge()
    {
        if (gatherGaugeUI)
            gatherGaugeUI.SetActive(false);
    }

    public void ShowEscapeResultUI(int reward, bool success)
    {
        HideAllUI();
        escapeResultUI.SetActive(true);
    }

    /// <summary>
    /// 상호작용 UI
    /// </summary>
    /// <param name="target"></param>
    /// <param name="text"></param>
    public void ShowInteractHint(Transform target, string text)
    {
        interactHint?.Show(target, text);
    }

    public void HideInteractHint()
    {
        interactHint?.Hide();
    }

    public UI_HUD hud;

    public void ShowGameUI()
    {
        HideAllUI();
        hudUI.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            Time.timeScale = inventoryUI.activeSelf ? 0f : 1f;
        }
    }

    public void HideAllUI()
    {
        if (hudUI) hudUI.SetActive(false);
        if (inventoryUI) inventoryUI.SetActive(false);
        if (gatherGaugeUI) gatherGaugeUI.SetActive(false);
        if (escapeResultUI) escapeResultUI.SetActive(false);
        if (shopUI) shopUI.SetActive(false);
        if (upgradeUI) upgradeUI.SetActive(false);
        if (gameOverUI) gameOverUI.SetActive(false);
        if (victoryUI) victoryUI.SetActive(false);
    }
}
