using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Inventory : MonoBehaviour
{
    [Header("Grid Settings")]
    [Tooltip("한 줄에 몇 칸인지")]
    public int columnCount = 4;

    [Header("Slots (순서 중요)")]
    public GameObject[] slots;

    int selectedIndex = -1;

    // 인벤토리 켜질 때
    void OnEnable()
    {
        Debug.Log("[UI_Inventory] ENABLED");

        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning("[UI_Inventory] Slots not assigned");
            return;
        }

        SelectFirstSlot();
    }

    void Update()
    {
        // 슬롯 없으면 아무것도 안 함
        if (slots == null || slots.Length == 0) return;

        // ← →
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLeft();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveRight();

        // ↑ ↓
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            MoveUp();

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            MoveDown();
    }

    // --------------------
    // 선택 관련
    // --------------------

    void SelectFirstSlot()
    {
        SetIndex(0);
    }

    void SetIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= slots.Length)
            return;

        selectedIndex = newIndex;
        OnSelectionChanged();
    }

    void OnSelectionChanged()
    {
        Debug.Log($"[UI_Inventory] Selected Slot : {selectedIndex}");

        // TODO (다음 단계)
        // - 이전 슬롯 색상 해제
        // - 현재 슬롯 강조
    }

    // --------------------
    // 이동 로직
    // --------------------

    void MoveLeft()
    {
        if (selectedIndex % columnCount == 0)
            return;

        SetIndex(selectedIndex - 1);
    }

    void MoveRight()
    {
        // 줄 끝
        if (selectedIndex % columnCount == columnCount - 1)
            return;

        // 슬롯 범위 초과 방지
        if (selectedIndex + 1 >= slots.Length)
            return;

        SetIndex(selectedIndex + 1);
    }

    void MoveUp()
    {
        int nextIndex = selectedIndex - columnCount;
        if (nextIndex < 0)
            return;

        SetIndex(nextIndex);
    }

    void MoveDown()
    {
        int nextIndex = selectedIndex + columnCount;
        if (nextIndex >= slots.Length)
            return;

        SetIndex(nextIndex);
    }
}