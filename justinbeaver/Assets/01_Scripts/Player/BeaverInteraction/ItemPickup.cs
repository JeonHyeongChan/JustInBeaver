using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable, IPoolable
{
    public string InteractText => "[Z] Get Item";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    [Header("Item")]
    [SerializeField] private ItemData itemData;
    public string itemId;


    private string ItemId => itemData != null ? itemData.itemId : null;


    public void Interact(PlayerController player)
    {
        Debug.Log("Item Get!");
        
        if (itemData == null || string.IsNullOrEmpty(itemData.itemId))
        {
            return;
        }

        if (!UIManager.Instance.TryAddItemToInventory(itemData.itemId))
        {
            return;
        }

        //풀 반환
        Despawn();
    }

    private void Despawn()
    {
        if (ObjectManager.Instance != null)
        {
            ObjectManager.Instance.DespawnObject(gameObject);
        }

        else if (PoolManager.Instance != null)
        {
            PoolManager.Instance.Despawn(gameObject);
        }    
            
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnSpawned() {}
    public void OnDespawned() {}
    public void OnHoldCancel(PlayerController player) {}
    public void OnHoldUpdate(PlayerController player, float progress01) {}
}
