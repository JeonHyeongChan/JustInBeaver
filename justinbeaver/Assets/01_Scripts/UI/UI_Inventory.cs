using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private UI_InventorySlot[] slots;
    [SerializeField] private RectTransform highlight;

    private BeaverController beaver;

    void OnEnable()
    {
        beaver = FindObjectOfType<BeaverController>();

        beaver.OnInventoryChanged += Refresh;
        beaver.OnSelectedSlotChanged += UpdateHighlight;

        Refresh(beaver.Inventory);
        UpdateHighlight(beaver.SelectedSlotIndex);
    }

    void OnDisable()
    {
        if (!beaver) return;
        beaver.OnInventoryChanged -= Refresh;
        beaver.OnSelectedSlotChanged -= UpdateHighlight;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            beaver.RequestMoveSlot(-1);

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            beaver.RequestMoveSlot(+1);
    }

    void Refresh(IReadOnlyList<InventoryItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
                slots[i].Set(items[i]);
            else
                slots[i].Clear();
        }
    }

    void UpdateHighlight(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        highlight.position = slots[index].transform.position;
    }
}