using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] Image background;

    void Awake()
    {
        if (background == null)
            background = GetComponent<Image>();
    }

    public void SetSelected(bool selected)
    {
        background.color = selected ? Color.yellow : Color.white;
    }
}