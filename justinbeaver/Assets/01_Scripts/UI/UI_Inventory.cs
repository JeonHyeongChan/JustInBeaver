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

        // 전부 선택 해제
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetSelected(false);

        Select(0);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // 이동
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            Move(-1, 0);
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            Move(1, 0);
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            Move(0, -1);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            Move(0, 1);

        // X 키 : 아이템 버리기
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            DropSelectedItem();
        }
    }

    void Move(int x, int y)
    {
        int row = selectedIndex / columnCount;
        int col = selectedIndex % columnCount;

        int nextRow = row + y;
        int nextCol = col + x;

        // 좌우 범위 체크
        if (nextCol < 0 || nextCol >= columnCount)
            return;

        int nextIndex = nextRow * columnCount + nextCol;

        // 슬롯 범위 체크
        if (nextIndex < 0 || nextIndex >= slots.Length)
            return;

        Select(nextIndex);
    }

    void Select(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        slots[selectedIndex].SetSelected(false);

        selectedIndex = index;

        slots[selectedIndex].SetSelected(true);

        Debug.Log($"▶ 선택 슬롯: {selectedIndex}");
    }

    void DropSelectedItem()
    {
        UI_InventorySlot slot = slots[selectedIndex];

        if (!slot.HasItem())
        {
            Debug.Log(" 버릴 아이템이 없음");
            return;
        }

        Debug.Log($" 아이템 버림 : {slot.GetItemName()}");

        // UI 상에서 제거 (실제 드롭은 Player 쪽에서)
        slot.Clear();
    }
}