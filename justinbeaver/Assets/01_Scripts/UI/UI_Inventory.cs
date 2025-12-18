using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Inventory : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columnCount = 4;

    [Header("Slots")]
    [SerializeField] UI_InventorySlot[] slots;

    int selectedIndex = 0;

    void OnEnable()
    {
        if (slots == null || slots.Length == 0) return;
        SelectSlot(0);
    }

    void Update()
    {
        //  나중에 Player 입력으로 대체 가능
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLeft();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveRight();

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            MoveUp();

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            MoveDown();
    }

    // --------------------
    // 이동 로직
    // --------------------

    void MoveLeft()
    {
        if (selectedIndex % columnCount == 0) return;
        SelectSlot(selectedIndex - 1);
    }

    void MoveRight()
    {
        if (selectedIndex % columnCount == columnCount - 1) return;
        if (selectedIndex + 1 >= slots.Length) return;
        SelectSlot(selectedIndex + 1);
    }

    void MoveUp()
    {
        int next = selectedIndex - columnCount;
        if (next < 0) return;
        SelectSlot(next);
    }

    void MoveDown()
    {
        int next = selectedIndex + columnCount;
        if (next >= slots.Length) return;
        SelectSlot(next);
    }

    // --------------------
    // 선택 처리
    // --------------------

    void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;

        slots[selectedIndex].SetSelected(false);
        selectedIndex = index;
        slots[selectedIndex].SetSelected(true);

        Debug.Log($"[Inventory] Selected Slot : {selectedIndex}");
    }
}