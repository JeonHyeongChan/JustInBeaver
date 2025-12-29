using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Button upgradeButton;
    public Button ExitButton;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        ExitButton.onClick.AddListener(OnClickExitButton);
    }

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
    }
    public void OnClickExitButton()
    {
        UIManager.Instance.HideShopUI();
    }
}
