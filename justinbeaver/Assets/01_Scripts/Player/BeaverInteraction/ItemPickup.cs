using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable, IPoolable
{
    public string InteractText => "[Z] Get Item";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    public void Interact(PlayerController player)
    {
        Debug.Log("Item Get!");
        //상호작용 필요한 것들

        // TODO: 인벤토리에 아이템 추가

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
