using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("InGame")]
    public GameObject hudUI;
    public GameObject inventoryUI;
    public GameObject gatherGaugeUI;
    public GameObject escapeResultUI;
    public GameObject pauseUI;
    

    public UI_GatherGauge gatherGauge;
    public UI_InteractHint interactHint;
    public UI_ItemTooltip itemTooltip;
    public UI_PlayerHearts playerHeart;
    public UI_WeightGauge weightGauge;
    public UI_HelpPanel helpPanel;

    private PlayerHealth playerHealth;      //구독 해제용
    private PlayerStatsManager boundStats;

    public GameObject gameFailUI;
    public GameObject gameSuccessUI;

    [Header("OutGame")]
    public GameObject shopUI;
    public GameObject upgradeUI;

    [Header("Common")]
    public GameObject gameOverUI;
    public GameObject victoryUI;

    
    private const string homeScene = "BeaverHouseScene";

    public UI_GatherGauge GatherGauge => gatherGauge;

    public UI_HelpPanel GetHelpPanel() => helpPanel;

    public bool IsInventoryOpen => inventoryUI != null && inventoryUI.activeSelf;
    public bool IsPauseOpen => pauseUI != null && pauseUI.activeSelf;
    



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            BindSceneUI();
            BindWeightEvents();
            HideAllUI();
        }
        else
        {
            Destroy(gameObject);    // 싱글톤으로 수정
        }
    }

    private void OnEnable()
    {
        if (PlayerStatsManager.Instance != null)
        {
            PlayerStatsManager.Instance.OnWeightChanged += HandleWeightChanged;
        }   
    }

    private void OnDisable()
    {
        if (PlayerStatsManager.Instance != null)
        {
            PlayerStatsManager.Instance.OnWeightChanged -= HandleWeightChanged;
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
        BindWeightEvents();
        SetInventoryOpen(false);
        HideItemTooltip();
        HideAllUI();
        ForceCloseUIState();

        if (scene.name == homeScene)
        {
            PlayerStatsManager.Instance?.ResetWeightToZero();
            RefreshWeightGauge();
        }
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
        BindItemTooltip();
        BindWeightGauge();
        BindWeightEvents();
        BindGameFailUI();
        BindGameSuccessUI();
        BindShopUI();
        BindUpgradeUI();
        BindPauseUI();
        BindHelpUI();
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
        if (playerHeart == null)
        {
            return;
        }

        //씬에 PlayerHealth 찾기
        var health = FindAnyObjectByType<PlayerHealth>(FindObjectsInactive.Exclude);
        if (health == null)
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


    private void BindGameFailUI()
    {
        Debug.Log("BindGameFailUI");
        gameFailUI = null;

        var marker = FindAnyObjectByType<GameFailUIMarker>(FindObjectsInactive.Include);
        if (marker == null)
        {
            gameFailUI = null;
            return;
        }

        gameFailUI = marker.gameObject;
        gameFailUI.SetActive(false);
    }

    private void BindGameSuccessUI()
    {
        Debug.Log("BindGameSuccessUI");
        gameSuccessUI = null;

        var marker = FindAnyObjectByType<GameSuccessUIMarker>(FindObjectsInactive.Include);
        if (marker == null)
        {
            gameSuccessUI = null;
            return;
        }

        gameSuccessUI = marker.gameObject;
        gameSuccessUI.SetActive(false);
    }


    private void BindItemTooltip()
    {
        itemTooltip = FindAnyObjectByType<UI_ItemTooltip>(FindObjectsInactive.Include);
        if (itemTooltip == null)
        {
            return;
        }
        itemTooltip.Hide();
    }


    private void BindWeightGauge()
    {
        weightGauge = FindAnyObjectByType<UI_WeightGauge>(FindObjectsInactive.Include);
        if (weightGauge == null)
        {
            return;
        }
        RefreshWeightGauge(); // 씬 로드 직후 한번 반영
    }


    private void BindShopUI()
    {
        shopUI = null;
        var marker = FindAnyObjectByType<UI_Shop>(FindObjectsInactive.Include);
        if (marker == null)
        {
            shopUI = null;
            return;
        }
        shopUI = marker.gameObject;
        shopUI.SetActive(false);
    }


    private void BindUpgradeUI()
    {
        upgradeUI = null;
        var maker = FindAnyObjectByType<UI_Upgrade>(FindObjectsInactive.Include);
        if (maker == null)
        {
            upgradeUI = null;
            return;
        }
        upgradeUI = maker.gameObject;
        upgradeUI.SetActive(false);
    }


    private void BindPauseUI()
    {
        pauseUI = null;
        var marker = FindAnyObjectByType<PauseUIMarker>(FindObjectsInactive.Include);
        if (marker == null)
        {
            pauseUI = null;
            return;
        }

        pauseUI = marker.gameObject;
        pauseUI.SetActive(false);
    }


    private void BindHelpUI()
    {
        helpPanel = FindAnyObjectByType<UI_HelpPanel>(FindObjectsInactive.Include);
        if (helpPanel != null)
        {
            helpPanel.gameObject.SetActive(false);
        }
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

        // 이미 원하는 상태면 아무것도 안 함
        if (inventoryUI.activeSelf == open)
        {
            return;
        }

        inventoryUI.SetActive(open);

        if (!open)
        {
            HideItemTooltip();
        }

        // 플레이어 잠금/해제
        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        if (player != null)
        {
            player.SetInventoryLocked(open);
            player.SetInventoryOpen(open);
        }
    }

    public void MoveInventoryCursor(Vector2 dir)
    {
        if (inventoryUI == null || !inventoryUI.activeSelf)
        {
            return;
        }
            
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


    public bool TryAddItemToInventory(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return false;
        }


        var grid = FindAnyObjectByType<Inventory_Grid>(FindObjectsInactive.Include);
        if (grid == null)
        {
            return false;
        }

        grid.RebindSlots();
        bool ok = grid.TryAddItem(itemId);
        if(ok)
{
            RunLootTracker.Instance?.AddItem(itemId, 1);
        }

        return ok;
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

    /// <summary>
    /// 비버 사망시 실패UI
    /// </summary>
    public void ShowGameFailUI()
    {
        if (gameFailUI != null)
            gameFailUI.SetActive(true);

        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(true);   // 플레이어 입력 잠그기

        var button = gameFailUI.GetComponentInChildren<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    public void ConfirmGameFail()
    {
        Debug.Log("ConfirmGameFail CLICKED");

        if (gameFailUI != null)
            gameFailUI.SetActive(false);
        RunLootTracker.Instance?.Clear();

        GameManager.Instance.HandlePlayerRespawn();
    }

    /// <summary>
    /// 비버 탈출시 성공UI
    /// </summary>
    public void ShowEscapeSuccessUI()
    {
        if (gameSuccessUI != null)
            gameSuccessUI.SetActive(true);

        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(true);

        var button = gameSuccessUI.GetComponentInChildren<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
       
    }

    /// <summary>
    /// 상점UI
    /// </summary>
    public void ShowShopUI()
    {
        if (shopUI == null)
        {
            return;
        }            
        shopUI.SetActive(true);

        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(true);

        FocusFirstSelectable(shopUI);
    }


    /// <summary>
    /// 스텟 업그레이드 UI
    /// </summary>
    public void ShowUpgradeUI()
    {
      
        if (upgradeUI != null)
        {
            HideShopUI();
            upgradeUI.SetActive(true);
          
        }

        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(true);

        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(DelaySelectFirstButton());
    }
    private IEnumerator DelaySelectFirstButton()
    {
        yield return null; // 다음 프레임
        FocusFirstSelectable(upgradeUI);
    }
    public void HideShopUI()
    {
        if (shopUI != null)
            shopUI.SetActive(false);

        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(false);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
    public void HideUpgradeUI()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(false);
        }
        var player = FindAnyObjectByType<PlayerController>();
        player?.SetInputLocked(false);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
    public void ConfirmEscapeSuccess()
    {
        Debug.Log("ConfirmEscapeSuccess CLICKED");

        if (gameSuccessUI != null)
            gameSuccessUI.SetActive(false);

        GameManager.Instance.HandlePlayerRespawn();
        RunLootTracker.Instance?.Clear();
    }


    /// <summary>
    /// 툴팁 UI
    /// </summary>
    public void ShowItemTooltip(UI_InventorySlot slot)
    {
        if (itemTooltip == null || slot == null || !slot.HasItem())
        {
            itemTooltip?.Hide();
            return;
        }

        var data = ItemManager.Instance != null ? ItemManager.Instance.GetItem(slot.GetItemId()) : null;
        itemTooltip.Show(data, slot.GetCount());
    }

    public void HideItemTooltip()
    {
        itemTooltip?.Hide();
    }


    /// <summary>
    /// 일시정지 UI
    /// </summary>
    public void ShowPauseUI()
    {
        if (pauseUI == null) return;

        pauseUI.SetActive(true);
        StopPlayerMotion();
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FocusFirstSelectable(pauseUI);
    }



    public void HidePauseUI()
    {
        if (pauseUI == null)
        {
            return;
        }

        pauseUI.SetActive(false);
        Time.timeScale = 1f;

        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        bool keepLocked = HasAnyModalOpenExceptPause();

        if (player != null)
        {
            player.SetInputLocked(keepLocked);
        }
        RestoreFocusToTopmostUI();

        if (keepLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    public void TogglePauseUI()
    {
        if (pauseUI == null)
        {
            return;
        }

        if (pauseUI.activeSelf)
        {
            HidePauseUI();
        }
        else
        {
            ShowPauseUI();
        }
    }


    public UI_HUD hud;
    public void ShowEscapeResultUI(int reward, bool success)
    {
        HideAllUI();
        escapeResultUI.SetActive(true);
    }

    public void ShowGameUI()
    {
        HideAllUI();
        hudUI.SetActive(true);
        RefreshWeightGauge();
    }

    public void HideAllUI()
    {
        if (hudUI) hudUI.SetActive(false);
        if (inventoryUI) inventoryUI.SetActive(false);
        if (gatherGaugeUI) gatherGaugeUI.SetActive(false);
        if (escapeResultUI) escapeResultUI.SetActive(false);
        if (upgradeUI) upgradeUI.SetActive(false);
        if (gameOverUI) gameOverUI.SetActive(false);
        if (victoryUI) victoryUI.SetActive(false);
        if (pauseUI) pauseUI.SetActive(false);
    }

    public Dictionary<string, int> CollectInventoryItems()  //(스토리지 매니저 연동) 인벤 아이템id들을 반환 id => 개수
    {
        var result = new Dictionary<string, int>();

        var grid = FindAnyObjectByType<Inventory_Grid>(FindObjectsInactive.Include);  //꺼져 있어도 반드시 잡아와라
        if (grid == null) return result;
        grid.RebindSlots();  //씬 전환해도 끊김 없게 

        foreach (var slot in grid.slots)
        {
            if (slot == null || !slot.HasItem()) continue;

            string id = slot.GetItemId();
            if (string.IsNullOrEmpty(id)) continue;

            if (!result.ContainsKey(id))
            {
                result[id] = 0;
            }
            result[id] += slot.GetCount();
        }
        return result;
    }


    public void ClearInventoryAll()  //(스테이지 매니저 연동) 인벤 슬롯 전체 비우기
    {
        var grid = FindAnyObjectByType<Inventory_Grid>(FindObjectsInactive.Include);
        
        if (grid == null)
        {
            return;
        }

        grid.RebindSlots();

        foreach (var slot in grid.slots)
        {
            if (slot != null && slot.HasItem())
            {
                slot.Clear();
            }
        }
    }


    public void RemoveItemsFromInventoryByFilter(Func<string, bool> filter)
    {
        var grid = FindAnyObjectByType<Inventory_Grid>(FindObjectsInactive.Include);
        if (grid == null)
        {
            return;
        }

        grid.RebindSlots();

        foreach (var slot in grid.slots)
        {
            if (slot == null || !slot.HasItem())
            {
                continue;
            }

            string id = slot.GetItemId();
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }    
                
            if (filter != null && filter(id))
            {
                slot.Clear();
            }
        }
    }


    private void HandleWeightChanged(float weight)
    {
        RefreshWeightGauge();
    }


    public void RefreshWeightGauge()
    {
        if (weightGauge == null)
        {
            return;
        }
        
        var stats = PlayerStatsManager.Instance;
        
        if (stats == null)
        {
            return;
        }
        weightGauge.Refresh(stats.CurrentWeight, stats.MaxWeight);
    }


    private void BindWeightEvents()
    {
        var stats = PlayerStatsManager.Instance;

        //이미 올바른 대상에 바인딩 되어있으면 끝
        if (boundStats == stats)
        {
            return;
        }

        //이전 대상 구독 해제
        if (boundStats != null)
        {
            boundStats.OnWeightChanged -= HandleWeightChanged;
        }
        boundStats = stats;

        //새 대상 구독
        if (boundStats != null)
        {
            boundStats.OnWeightChanged += HandleWeightChanged;
        }
        //즉시 1회 반영
        RefreshWeightGauge();
    }

    public void HandlePauseBack()
    {
        if (pauseUI == null)
        {
            return;
        }

        var pause = pauseUI.GetComponent<UI_Pause>();
        if (pause == null)
        {
            return;
        }

        //UI_Pause 내부에서 현재 패널 상태 판단
        if (pause.IsInSubPanel)
        {
            pause.OnClickBack();
        }
        else
        {
            HidePauseUI();
        }
    }

    private void StopPlayerMotion()
    {
        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        if (player == null) return;

        player.SetInputLocked(true);

        // Rigidbody 기반 정지
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }



    //첫번째 버튼 찾기
    private void FocusFirstSelectable(GameObject root)
    {
        if (root == null || EventSystem.current == null)
        {
            return;
        }

        var buttons = root.GetComponentsInChildren<Button>(true);
        Button targetBtn = null;
        foreach (var button in buttons)
        {
            if (button != null && button.gameObject.activeInHierarchy && button.interactable)
            {
                targetBtn = button;
                break;
            }
        }

        Selectable target = targetBtn != null
            ? targetBtn
            : root.GetComponentInChildren<Selectable>(true);

        if (target == null)
        {
            return;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target.gameObject);
    }


    private void ForceCloseUIState()
    {
        //시간 정상화 (Pause가 켜져있던 상태를 강제로 해제)
        Time.timeScale = 1f;

        //Pause 패널 강제 OFF
        if (pauseUI != null) pauseUI.SetActive(false);

        if (shopUI != null) shopUI.SetActive(false);
        if (upgradeUI != null) upgradeUI.SetActive(false);

        //플레이어 입력 잠금 해제 + 입력맵 Player로 복귀(네가 맵 스위칭 쓰는 경우)
        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        player?.SetInputLocked(false);

        //UI 선택 초기화
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }


    private bool HasAnyModalOpenExceptPause()
    {
        //pauseUI 제외, 플레이어 조작을 막아야 하는 UI들 체크
        if (inventoryUI != null && inventoryUI.activeSelf)
        {
            return true;
        }    
            
        if (shopUI != null && shopUI.activeSelf)
        {
            return true;
        }    
            
        if (upgradeUI != null && upgradeUI.activeSelf)
        {
            return true;
        }    
            
        if (gameFailUI != null && gameFailUI.activeSelf)
        {
            return true;
        }    
            
        if (gameSuccessUI != null && gameSuccessUI.activeSelf)
        {
            return true;
        }
        return false;
    }


    private void RestoreFocusToTopmostUI()
    {
        // 우선순위, 실패/성공 > 업그레이드 > 상점 > 인벤
        if (gameFailUI != null && gameFailUI.activeSelf)
        {
            FocusFirstSelectable(gameFailUI);
            return;
        }

        if (gameSuccessUI != null && gameSuccessUI.activeSelf)
        {
            FocusFirstSelectable(gameSuccessUI);
            return;
        }

        if (upgradeUI != null && upgradeUI.activeSelf)
        {
            FocusFirstSelectable(upgradeUI);
            return;
        }

        if (shopUI != null && shopUI.activeSelf)
        {
            FocusFirstSelectable(shopUI);
            return;
        }

        if (inventoryUI != null && inventoryUI.activeSelf)
        {
            FocusFirstSelectable(inventoryUI);
            return;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}