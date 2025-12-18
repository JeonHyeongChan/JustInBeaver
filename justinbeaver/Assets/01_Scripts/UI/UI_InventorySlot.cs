using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] Image background;

    public void SetSelected(bool selected)
    {
        if (background == null)
        {
            Debug.LogError($"{name} : background Image ¿¬°á ¾ÈµÊ");
            return;
        }

        background.color = selected ? Color.yellow : Color.white;
    }
}