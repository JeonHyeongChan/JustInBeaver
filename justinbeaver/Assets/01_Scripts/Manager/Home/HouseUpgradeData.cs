using UnityEngine;

[CreateAssetMenu(fileName = "HouseUpgradeData", menuName = "Scriptable Objects/HouseUpgradeData")]
public class HouseUpgradeData : ScriptableObject
{
    [Header("Upgrade Result")]
    public int targetLevel; // 2, 3, 4, 5

    [Header("Required Materials")]
    public MaterialCost[] requiredMaterials;
}
