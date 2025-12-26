using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    void Update()
    {
        
    }

    public void OnClickUpgradeButton()
    {
        UIManager.Instance.ShowUpgradeUI();
        UIManager.Instance.HideShopUI();
    }
    public void OnClickExitButton()
    {
        UIManager.Instance.HideShopUI();
    }
}
