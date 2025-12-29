using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int houseLevel;
    public int failCountAtcurrentLevel;

    public List<StoredItem> storedItems = new();
}

[Serializable]
public class StoredItem
{
    public string itemId;
    public int count;
}
