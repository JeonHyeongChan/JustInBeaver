using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject inventoryUI;

    public event Action<bool> OnInventoryVisibilityChanged;

    void Awake()
    {
        Instance = this;
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Tab : 인벤토리 열기
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OpenInventory();
        }

        // X : 인벤토리 닫기
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            CloseInventory();
        }
    }

    void OpenInventory()
    {
        if (inventoryUI.activeSelf) return;

        inventoryUI.SetActive(true);
        OnInventoryVisibilityChanged?.Invoke(true);
    }

    void CloseInventory()
    {
        if (!inventoryUI.activeSelf) return;

        inventoryUI.SetActive(false);
        OnInventoryVisibilityChanged?.Invoke(false);
    }
}