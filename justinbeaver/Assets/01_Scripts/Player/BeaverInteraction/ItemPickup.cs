using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        Debug.Log("Item Get!");
        //상호작용 필요한 것들

        Destroy(gameObject);
    }   
}
