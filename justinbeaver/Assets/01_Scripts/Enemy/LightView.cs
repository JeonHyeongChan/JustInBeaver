using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
public struct CastInfo
{
    public bool Hit;                // ** 맞았는지 확인

    public Vector3 Point;           // ** 맞았다면 맞은 위치, 안맞았다면 Range 거리

    public float Distance;          // ** 도달 거리
    public float Angle;             // ** 각도
}


public class LightView : MonoBehaviour
{
    public float viewRange; 
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> targetList;
   
    //보이는 라이트뷰 정보
    public float angle;
    public List<CastInfo> lineList;       // 표시된 선의 정보 리스트
    public Vector3 offset; //위치 보정용 벡터.
    private void Awake()
    {
        targetList ??= new List<Transform>();
        lineList ??= new List<CastInfo>();
    }
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CheckTarget());
        StartCoroutine(DrawRayLine());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
        targetList?.Clear();
        lineList?.Clear();
    }
    IEnumerator CheckTarget()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        while (true)
        {
            targetList.Clear();
            Vector3 origin = transform.position + offset;
            Collider[] cols = Physics.OverlapSphere(origin, viewRange, targetMask);
            foreach (var e in cols)
            {
                //검출한 대상 방향 구하기
                Vector3 targetPos = e.transform.position + offset;
                Vector3 toTarget = targetPos - origin;
                Vector3 dir = toTarget.normalized;
              
                if (Vector3.Angle(transform.forward, dir) > (viewAngle * 0.5f))
                    continue;

                //각도 구하기
                //대상과의 각도가 설정한 각도 이내에 있는지 확인
                //부채꼴 이니까 0.5 곱해주기
                //레이캐스트 쏴서 장애물 있는지 검사
                float dist = toTarget.magnitude;
               
                if (Physics.Raycast(origin, dir, dist, obstacleMask))
                    continue;

                Debug.DrawLine(origin, targetPos, Color.red, 0.1f);
                targetList.Add(e.transform);
            }

            yield return wfs;
        }
    }

    IEnumerator DrawRayLine()
    {
        while(true)
        {
            lineList.Clear();  //이미 생성된 레이캐스트 정보 삭제

            //선이 표시될 갯수. 
            float step = Mathf.Max(0.5f, angle);
            int count = Mathf.RoundToInt(viewAngle / step) + 1;
            //각도 시작점. 시야각가 플레이어 방향 기준으로 결정
            float fAngle = (-viewAngle * 0.5f) + transform.eulerAngles.y;

            //표시될 선 갯수만큼 실행
            for (int i = 0; i < count; ++i)
            {
                //해당 각도로 발사한 레이캐트 정보 얻기
                CastInfo info = GetCastInfo(fAngle + (step * i));
                lineList.Add(info); //리스트에 추가

            }
            yield return null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;
        if (lineList == null || lineList.Count == 0) return;
        Gizmos.color = Color.green;
        foreach (var info in lineList)
        {
            Gizmos.DrawLine(transform.position + offset, info.Point);
        }
    }

    CastInfo GetCastInfo(float Angle)
    {
        //입력받은 각도에 따라 방향 벡터 구하기
        Vector3 dir = new Vector3(Mathf.Sin(Angle * Mathf.Deg2Rad), 0, Mathf.Cos(Angle * Mathf.Deg2Rad));
        CastInfo Info;
        RaycastHit hit;

        //장애물 맞는지 확인
        if (Physics.Raycast(transform.position + offset, dir, out hit, viewRange, obstacleMask))
        {
            Info.Hit = true;  // 장애물 맞았는지 참여부
            Info.Angle = Angle;  // 각도
            Info.Distance = hit.distance;  //거리
            Info.Point = hit.point;  //맞은 위치
        }
        else //장애물 안맞음
        {
            Info.Hit = false;
            Info.Angle = Angle;
            Info.Distance = viewRange;  //맞지 않았다면 최대 도달거리 Range
            Info.Point = transform.position + offset + dir * viewRange;   // 맞지 않았다면 해당 방향으로 최대 거리인 Range의 위치
        }
        return Info;
    }
    public bool HasTarget()
    {
        return targetList != null && targetList.Count > 0;
    }

    public Transform GetFirstTarget()
    {
        if (targetList == null || targetList.Count == 0) return null;
        return targetList[0];
    }

}
