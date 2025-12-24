using System;
using UnityEngine;

public class PlayerGatherSignal : MonoBehaviour
{
    // 갈무리 시작
    public event Action<Vector3> OnGatherStart;
    // 갈무리 끝/취소
    public event Action OnGatherEnd;

    public bool IsGathering { get; private set; }
    public Vector3 AnchorPos { get; private set; }

    public void RaiseGatherStart(Vector3 anchorPos)
    {
        IsGathering = true;
        AnchorPos = anchorPos;
        OnGatherStart?.Invoke(anchorPos);
    }

    public void RaiseGatherEnd()
    {
        IsGathering = false;
        OnGatherEnd?.Invoke();
    }
}
