using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/ItemDatabase", fileName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<string, ItemData> map;

    public ItemData Find(string id)
    {
        if (map == null)
        {
            map = new Dictionary<string, ItemData>();
            foreach (var it in items)
                if (it != null && !string.IsNullOrEmpty(it.itemId))
                    map[it.itemId] = it;
        }
        return map.TryGetValue(id, out var data) ? data : null;
    }
}
