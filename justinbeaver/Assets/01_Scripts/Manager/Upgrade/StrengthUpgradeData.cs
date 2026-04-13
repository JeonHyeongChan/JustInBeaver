using UnityEngine;

[CreateAssetMenu(fileName = "StrengthUpgradeData", menuName = "Scriptable Objects/StrengthUpgradeData")]
public class StrengthUpgradeData : ScriptableObject
{
    [Header("Level")]
    public int targetLevel = 1;              //이 데이터가 몇 레벨로 올리는지
    public float addStrength = 1f;           //클릭 시 Strength 증가량(혹은 레벨업당 증가량)

    [Header("Cost")]
    public MaterialCost[] requiredMaterials; //StorageManager가 검사/소모할 재료
}
