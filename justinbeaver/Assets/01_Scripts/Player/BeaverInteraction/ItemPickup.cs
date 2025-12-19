using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public string InteractText => "[Z] Get Item";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    public void Interact(PlayerController player)
    {
        Debug.Log("Item Get!");
        //상호작용 필요한 것들

        Destroy(gameObject);
    }

    public void OnHoldCancel(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        throw new System.NotImplementedException();
    }
}
