using UnityEngine;

public class CaptureObject : MonoBehaviour
{
    [Header("갈무리 설정")]
    public float gatherDuration = 2.0f;   //갈무리 시간(초)
    public bool consumeOnComplete = false;


    public void OnGatherComplete()
    {
        //테스트용: 완료 시 처리
        //오브젝트 풀링 매니저 구현 시 변경
        if (consumeOnComplete)
        {
            Destroy(gameObject);
            Debug.Log("갈무리 종료");
        }
    }
}
