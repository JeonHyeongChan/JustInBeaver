using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemTooltip : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private TMP_Text countText;

    public void Hide()
    {
        if (root) root.SetActive(false);
        else gameObject.SetActive(false);
    }

    public void Show(ItemData data, int count)
    {
        if (data == null)
        {
            Hide();
            return;
        }

        if (root)
        {
            root.SetActive(true);
        }    
        else
        {
            gameObject.SetActive(true);
        }

        if (icon)
        {
            icon.sprite = data.icon;
            icon.enabled = (data.icon != null);
        }

        if (nameText)
        {
            nameText.text = data.displayName;
        }    
            
        if (descText)
        {
            descText.text = data.description;
        }

        if (countText)
        {
            countText.gameObject.SetActive(count > 1);
            countText.text = $"x{count}";
        }
    }
}
