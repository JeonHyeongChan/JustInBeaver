using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Inventory : MonoBehaviour
{
    [Header("Grid")]
    public int columnCount = 3;
    public UI_InventorySlot[] slots;

    int selectedIndex = 0;

    void OnEnable()
    {
        if (slots == null || slots.Length == 0) return;
        Select(0);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            Move(-1, 0);
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            Move(1, 0);
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            Move(0, -1);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            Move(0, 1);
    }

    void Move(int x, int y)
    {
        int row = selectedIndex / columnCount;
        int col = selectedIndex % columnCount;

        int nextRow = row + y;
        int nextCol = col + x;

        if (nextCol < 0 || nextCol >= columnCount)
            return;

        int next = nextRow * columnCount + nextCol;

        if (next < 0 || next >= slots.Length)
            return;

        Select(next);
    }

    void Select(int index)
    {
        slots[selectedIndex].SetSelected(false);
        selectedIndex = index;
        slots[selectedIndex].SetSelected(true);

        Debug.Log($"º±≈√ ΩΩ∑‘: {selectedIndex}");
    }
}