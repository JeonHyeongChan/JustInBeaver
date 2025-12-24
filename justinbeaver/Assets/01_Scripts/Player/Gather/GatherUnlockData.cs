using UnityEngine;

public enum GatherCategory
{
    Wood,
    Stone,
    Metal,
    Glass
}

[CreateAssetMenu(menuName = "Game/Gather/Gather Unlock Data", fileName = "GatherUnlockData")]
public class GatherUnlockData : ScriptableObject
{
    [Header("이 갈무리 카테고리를 해금하는 최소 거점 레벨")]
    public GatherCategory category;
    [Range(1, 5)] public int requiredHomeLevel = 1;

    [Header("잠겼을 때 안내 문구")]
    public string lockedMessage = "거점 레벨이 부족합니다.";
}