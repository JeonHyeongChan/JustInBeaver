using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class UIManager : MonoBehaviour
{
    public event Action<bool> OnInventoryVisibilityChanged;

    public void RequestToggleInventory()
    {
        if (!inventoryUI) return;

        bool open = !inventoryUI.activeSelf;
        inventoryUI.SetActive(open);
        hudUI?.SetActive(true);

        OnInventoryVisibilityChanged?.Invoke(open);
    }



    public static UIManager Instance;

    public GameObject hudUI;
    public GameObject inventoryUI;

    public event Action<bool> OnInventoryVisibilityChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        HideAllUI();
    }

    public void RequestToggleInventory()
    {
        if (!inventoryUI) return;

        bool isOpen = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isOpen);
        hudUI?.SetActive(true);

        OnInventoryVisibilityChanged?.Invoke(!isOpen);
    }
}
