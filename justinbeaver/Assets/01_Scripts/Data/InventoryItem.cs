using UnityEngine;

public enum ItemType
{
    Wood,
    Stone,
    Iron
}

[System.Serializable]
public class InventoryItem
{
    public ItemType type;
    public int amount;

    public InventoryItem(ItemType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}

