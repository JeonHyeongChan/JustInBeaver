using UnityEngine;

[CreateAssetMenu(fileName = "HouseUpgradeData", menuName = "Scriptable Objects/HouseUpgradeData")]
public class HouseUpgradeData : ScriptableObject
{
    public int targetLevel;

    [Header("Required Materials")]
    public MaterialCost[] requiredMaterials;

    [Header("Visual")]
    public GameObject exteriorPrefab;

    [Header("Gameplay")]
    public int unlockFarmTier;
}

[System.Serializable]
public struct MaterialCost
{
    public string itemId;
    public int amount;
}
