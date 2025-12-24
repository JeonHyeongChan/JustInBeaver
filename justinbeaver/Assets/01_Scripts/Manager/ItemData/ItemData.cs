using UnityEngine;

public enum ItemType
{
    None,
    Resource,
    ingredient
}

[CreateAssetMenu(menuName = "Game/Items/ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public GameObject itemPrefab;
    public string itemId;
    public string itemName;

    [Header("Display")]
    public string displayName;
    [TextArea] public string description;

    [Header("Type")]
    public ItemType type = ItemType.ingredient;
    public bool stackable = true;
    public int maxStack = 99;

    [Header("Weight")]
    [Range(1, 5)] public float weight = 1f;
}
