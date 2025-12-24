using UnityEngine;

[CreateAssetMenu(menuName = "Game/Gather/Gather Unlock Data", fileName = "GatherUnlockData")]
public class GatherUnlockData : ScriptableObject
{
    [Range(1, 5)] public int requiredHomeLevel = 1;
    public string lockedMessage;
}