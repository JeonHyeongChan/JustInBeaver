using UnityEngine;

public class StorageTransfer : MonoBehaviour
{
    public void TransferInventoryStorage()  //탈출 성공하면 인벤 아이템 수거 => 창고에 추가 => 인벤 비우기
    {
        if (UIManager.Instance == null || StorageManager.Instance == null) 
            return;

        var items = UIManager.Instance.CollectInventoryItems();

        foreach (var item in items)
        {
            StorageManager.Instance.AddItems(item.Key, item.Value);
        }
        Debug.Log("[StorageTransfer]");

        UIManager.Instance.ClearInventoryAll();
    }

    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEscapeSucceeded += TransferInventoryStorage;
        }
            
    }

    private void OnDisable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEscapeSucceeded -= TransferInventoryStorage;
        }
    }
}
