using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [Header("UI")]
    public GameObject hudUI;
    public GameObject inventoryUI;

    // 인벤토리 열림/닫힘 이벤트
    public event Action<bool> OnInventoryVisibilityChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        bool isOpen = inventoryUI.activeSelf;

        inventoryUI.SetActive(!isOpen);
        hudUI.SetActive(true);

        OnInventoryVisibilityChanged?.Invoke(!isOpen);
    }
}