using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            Time.timeScale = inventoryPanel.activeSelf ? 0f : 1f;
        }
    }
}