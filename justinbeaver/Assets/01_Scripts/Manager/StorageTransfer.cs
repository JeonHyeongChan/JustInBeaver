using UnityEngine;
using UnityEngine.SceneManagement;

public class StorageTransfer : MonoBehaviour
{
    private RuleManager _boundRule;

    public void TransferInventoryStorage()  //탈출 성공하면 인벤 아이템 수거 => 창고에 추가 => 인벤 비우기
    {
   
        if (UIManager.Instance == null || StorageManager.Instance == null)
        {
            return;
        }

        var items = UIManager.Instance.CollectInventoryItems();

        foreach (var kv in items)
        {
            var data = ItemManager.Instance != null ? ItemManager.Instance.GetItem(kv.Key) : null;
            if (data == null)
            {
                continue;
            }

            //재료를 창고로
            if (data.type != ItemType.ingredient)
            {
                continue;
            }
            StorageManager.Instance.AddItems(kv.Key, kv.Value);
        }

        //이관된 재료는 인벤에서 제거
        UIManager.Instance.RemoveItemsFromInventoryByFilter(id =>
        {
            var d = ItemManager.Instance != null ? ItemManager.Instance.GetItem(id) : null;
            return d != null && d.type == ItemType.ingredient;
        });

        SaveAfterSuccess(); // 세이브

        Debug.Log("[StorageTransfer]");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        RebindRule();
    }

    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEscapeSucceeded += TransferInventoryStorage;
        }

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnbindRule();
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        RebindRule();
    }


    private void RebindRule()
    {
        var rule = RuleManager.Instance;
        if (rule == _boundRule)
        {
            return;
        }    
            

        UnbindRule();
        _boundRule = rule;

        if (_boundRule != null)
        {
            _boundRule.OnEscapeSucceeded += TransferInventoryStorage;
        }            
    }


    private void UnbindRule()
    {
        if (_boundRule != null)
            _boundRule.OnEscapeSucceeded -= TransferInventoryStorage;
        _boundRule = null;
    }

    /// <summary>
    /// 세이브
    /// </summary>
    private void SaveAfterSuccess()
    {
        var data = new SaveData();

        data.houseLevel = HomeManager.Instance.CurrentLevel;
        data.failCountAtcurrentLevel = RuleManager.Instance.GetEscapeFailCount();
        data.storedItems = StorageManager.Instance.ExportSaveData();

        SaveManager.Save(data);
    }
}
