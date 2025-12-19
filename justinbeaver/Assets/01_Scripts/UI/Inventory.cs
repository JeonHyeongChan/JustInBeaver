using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public GameObject Inventory_Obj;

    public event Action<bool> OnInventoryVisibilityChanged;

    void Awake()
    {
        Instance = this;
        Inventory_Obj.SetActive(false);
    }



    void OpenInventory()
    {
        if (Inventory_Obj.activeSelf) return;

        Inventory_Obj.SetActive(true);
        OnInventoryVisibilityChanged?.Invoke(true);
    }

    void CloseInventory()
    {
        if (!Inventory_Obj.activeSelf) return;

        Inventory_Obj.SetActive(false);
        OnInventoryVisibilityChanged?.Invoke(false);
    }
}
