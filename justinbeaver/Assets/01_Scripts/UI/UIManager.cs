using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public UI_GatherGauge gatherGauge;
    public UI_InteractHint interactHint;
    public UI_ItemTooltip itemTooltip;
    public UI_PlayerHearts playerHeart;

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

    public UI_WeightGauge weightGauge;


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
        var root = FindAnyObjectByType<InventoryRootMarker>(FindObjectsInactive.Include);
        if (root == null)
        {
            inventoryUI = null;
            return;
        }

        inventoryUI = root.gameObject;
        inventoryUI.SetActive(false); //시작 시 무조건 OFF
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

    public void ConfirmEscapeSuccess()
    {
        Debug.Log("ConfirmEscapeSuccess CLICKED");

        if (gameSuccessUI != null)
            gameSuccessUI.SetActive(false);

        GameManager.Instance.HandlePlayerRespawn();
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
        if (shopUI) shopUI.SetActive(false);
        if (upgradeUI) upgradeUI.SetActive(false);
        if (gameOverUI) gameOverUI.SetActive(false);
        if (victoryUI) victoryUI.SetActive(false);
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
}
