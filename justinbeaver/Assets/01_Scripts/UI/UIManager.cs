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

    public UI_PlayerHearts playerHeart;
    private PlayerHealth playerHealth;      //구독 해제용

    [Header("OutGame")]
    public GameObject shopUI;
    public GameObject upgradeUI;

    [Header("Common")]
    public GameObject gameOverUI;
    public GameObject victoryUI;


    public UI_GatherGauge GatherGauge => gatherGauge;
    public bool IsInventoryOpen => inventoryUI != null && inventoryUI.activeSelf;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            BindSceneUI();
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

        if (playerHealth != null && playerHeart != null)
        {
            playerHealth.OnHealthChanged -= playerHeart.SetHeart;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindSceneUI();
        HideAllUI();
    }

    /// <summary>
    /// 씬 전환 시 모든 씬 UI 재바인딩
    /// </summary>
    private void BindSceneUI()
    {
        BindGatherGauge();
        BindInteractHint();
        BindHearts();
        BindInventory();
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

    private void BindHearts()
    {
        //씬에 하트 UI 찾기
        playerHeart = FindAnyObjectByType<UI_PlayerHearts>(FindObjectsInactive.Include);
        if(playerHeart == null)
        {
            return;
        }

        //씬에 PlayerHealth 찾기
        var health = FindAnyObjectByType<PlayerHealth>(FindObjectsInactive.Exclude);
        if(health == null)
        {
            return;
        }

        //이전 구독 해제
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= playerHeart.SetHeart; 
        }

        playerHealth = health;
        playerHealth.OnHealthChanged += playerHeart.SetHeart;

        //즉시 반영
        playerHeart.SetHeart(playerHealth.currentHealth, playerHealth.maxHealth);
    }

    private void BindInventory()
    {
        var grid = FindAnyObjectByType<Inventory_Grid>(FindObjectsInactive.Include);
        if (grid == null)
        {
            inventoryUI = null;
            return;
        }

        Transform taget = grid.transform;

        while (taget.parent != null)
        {
            if (taget.parent.GetComponent<Canvas>() != null)
            {
                break;
            }
            taget = taget.parent;
        }
        inventoryUI = taget.gameObject;

        //시작 시 강제 OFF
        inventoryUI.SetActive(false);
    }

    public void ToggleInventory()
    {
        if (inventoryUI == null)
        {
            return;
        }
        SetInventoryOpen(!inventoryUI.activeSelf);
    }


    public void SetInventoryOpen(bool open)
    {
        if (inventoryUI == null)
        {
            return;
        }

        if (inventoryUI.activeSelf == open)
        {
            return;
        }
        inventoryUI.SetActive(open);

        //플레이어 잠금/해제
        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        if (player != null)
        {
            player.SetInputLocked(open);
            player.SetInventoryOpen(open);
        }
    }



    public void MoveInventoryCursor(Vector2 dir)
    {
        var grid = inventoryUI.GetComponentInChildren<Inventory_Grid>(true);
        if (grid == null)
        {
            return;
        }

        int x = Mathf.RoundToInt(dir.x);
        int y = Mathf.RoundToInt(dir.y);
        grid.Move(x, -y); //UI 좌표 보정
    }

    public void DropSelectedItem()
    {
        var grid = inventoryUI.GetComponentInChildren<Inventory_Grid>(true);
        grid?.DropSelectedItem();
    }



    /// <summary>s
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
