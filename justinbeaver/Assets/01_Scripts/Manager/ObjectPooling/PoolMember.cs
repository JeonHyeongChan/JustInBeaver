using UnityEngine;

//자신이 어떤 프리팹 풀로 돌아가야 하는지 기록
public class PoolMember : MonoBehaviour
{
    public int prefabId;

    //중복 Release 방지용
    [HideInInspector] public bool isInPool;
}
