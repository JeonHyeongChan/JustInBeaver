using UnityEngine;

public class AirSnapTrigger : MonoBehaviour
{

    //플레이어를 이 속도로 아래로 떨어뜨림
    public float forcedDownVelocity = -15f;

    //닿는 순간 Y속도를 0으로 만들지 여부
    public bool zeroUpVelocityFirst = true;

    private void OnTriggerEnter(Collider other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (pc == null) return;

        pc.ForceDrop(forcedDownVelocity, zeroUpVelocityFirst);
    }
}