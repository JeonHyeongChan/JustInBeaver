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

    private void BindSceneUI()
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
