using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    
    void Start()
    {
      
    }

   
    void Update()
    {
        
    }

    public void OnClickStatUpgradeButton()
    {
        //스텟업글
    }
    public void OnClickHouseUpgradeButton()
    {
        if (HomeManager.Instance == null)
        {
            Debug.LogWarning("HomeManager not found");
            return;
        }

        if (!HomeManager.Instance.CanUpgrade())
        {
            Debug.Log("House already at MAX level");
            return;
        }

        bool success = HomeManager.Instance.TryUpgradeHome();

        if (success)
        {
            Debug.Log($"House upgrade Lv.{HomeManager.Instance.CurrentLevel}");
        }
        else
        {
            Debug.Log("House upgrade failed");
        }
    }
    public void OnClickExitButton()
    {
        UIManager.Instance.HideUpgradeUI();
    }
}
