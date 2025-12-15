using UnityEngine;

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

    void Awake()
    {
        Instance = this;
        HideAllUI();
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
