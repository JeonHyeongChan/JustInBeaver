using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Inventory : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columnCount = 4;

    [Header("Slot Objects")]
    public GameObject[] slots;

    int selectedIndex = -1;

    void OnEnable()
    {
        SelectFirstSlot();
    }

    void Update()
    {
        if (slots == null || slots.Length == 0) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLeft();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveRight();

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            MoveUp();

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            MoveDown();
    }

    void SelectFirstSlot()
    {
        selectedIndex = 0;
        OnSelectionChanged();
    }

    // --------------------
    // 이동 로직
    // --------------------

    void MoveLeft()
    {
        if (selectedIndex % columnCount == 0) return;
        SetIndex(selectedIndex - 1);
    }

    void MoveRight()
    {
        if (selectedIndex % columnCount == columnCount - 1) return;
        if (selectedIndex + 1 >= slots.Length) return;

        SetIndex(selectedIndex + 1);
    }

    void MoveUp()
    {
        int nextIndex = selectedIndex - columnCount;
        if (nextIndex < 0) return;

        SetIndex(nextIndex);
    }

    void MoveDown()
    {
        int nextIndex = selectedIndex + columnCount;
        if (nextIndex >= slots.Length) return;

        SetIndex(nextIndex);
    }

    void SetIndex(int newIndex)
    {
        selectedIndex = newIndex;
        OnSelectionChanged();
    }

    void OnSelectionChanged()
    {
        Debug.Log($"[Inventory] Selected Slot : {selectedIndex}");

        // TODO:
        // - 이전 슬롯 강조 해제
        // - 현재 슬롯 강조
    }
}