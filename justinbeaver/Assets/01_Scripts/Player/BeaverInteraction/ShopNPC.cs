using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    public string InteractText => "[Z] Upgrade House";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    public void Interact(PlayerController player)
    {
        Debug.Log("NPC with talking");
        UIManager.Instance.ShowShopUI();
        //  if (HomeManager.Instance == null)
        //  {
        //      Debug.LogWarning("HomeManager not found");
        //      return;
        //  }
        //
        //  if (!HomeManager.Instance.CanUpgrade())
        //  {
        //      Debug.Log("House already at MAX level");
        //      return;
        //  }
        //
        //  bool success = HomeManager.Instance.TryUpgradeHome();
        //
        //  if (success)
        //  {
        //      Debug.Log($"House upgrade Lv.{HomeManager.Instance.CurrentLevel}");
        //  }
        //  else
        //  {
        //      Debug.Log("House upgrade failed");
        //  }
    }

    public void OnHoldCancel(PlayerController player)
    {
        
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        
    }
}
