using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable, IPoolable
{
    public string InteractText => "[Z] Get Item";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    private bool _consumed;


    [Header("Item")]
    [SerializeField] private ItemData itemData;
    public string itemId;


    private string ItemId => itemData != null ? itemData.itemId : null;


    public void OnSpawned()
    {
        _consumed = false; //풀에서 나올 때 초기화
    }


    public void Interact(PlayerController player)
    {
        //중복 방지
        if (_consumed)
        {
            return;
        }    
                 
        _consumed = true;

        string id = (itemData != null && !string.IsNullOrEmpty(itemData.itemId))
            ? itemData.itemId
            : itemId;

        if (string.IsNullOrEmpty(id))
        {
            _consumed = false;
            return;
        }

        if (!UIManager.Instance.TryAddItemToInventory(id))
        {
            _consumed = false; //인벤토리 가득 참이면 다시 주울 수 있게
            return;
        }

        SoundManager.Instance?.PlaySFX(SFXType.ItemCollect); // 사운드

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

    
    public void OnDespawned() {}
    public void OnHoldCancel(PlayerController player) {}
    public void OnHoldUpdate(PlayerController player, float progress01) {}
}
