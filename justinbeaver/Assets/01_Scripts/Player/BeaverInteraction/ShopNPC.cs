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
    }

    public void OnHoldCancel(PlayerController player)
    {
        
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        
    }
}
