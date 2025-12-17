using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    public GameObject itemPrefab;
    public string itemId;
    public string itemName;
    public float  itemWeight = 1f;   
}
