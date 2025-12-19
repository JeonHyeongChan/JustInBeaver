using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    public string InteractText => "[Z] Open Shop";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    public void Interact(PlayerController player)
    {
        Debug.Log("NPC with talking");
        //상호작용 필요한 것들
    }

    public void OnHoldCancel(PlayerController player)
    {
        
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        
    }
}
