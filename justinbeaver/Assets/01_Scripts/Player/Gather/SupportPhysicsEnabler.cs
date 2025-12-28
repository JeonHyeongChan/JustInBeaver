using UnityEngine;
using System.Collections;


public class SupportPhysicsEnabler : MonoBehaviour
{
    [Header("벽 레이어")]
    [SerializeField] private LayerMask supportMask;

    [Header("드롭 조건")]
    [SerializeField] private float unsupportedTimeToEnable = 0.2f;

    [Header("Checks")]
    [SerializeField] private float wallCheckDistance = 0.03f;
    [SerializeField] private float sideCheckDistance = 0.03f;
    [SerializeField] private float checkRadius = 0.08f;

    [HideInInspector] private bool ignoreTriggers = true;
    [HideInInspector] private float startDelay = 0.15f;
    [HideInInspector] private int fixedFramesDelay = 2;

    private Rigidbody wallRigidbody;
    private Collider wallCollider;

    private bool canEvaluate;
    private bool physicsEnabled;
    private float unsupportedTimer;


    private void Awake()
    {
        wallRigidbody = GetComponent<Rigidbody>();
        wallCollider = GetComponent<Collider>();
        DisablePhysics(); //기본은 물리 OFF
    }

    private void OnEnable()
    {
        canEvaluate = false;
        physicsEnabled = false;
        unsupportedTimer = 0f;

        DisablePhysics();
        StartCoroutine(CoEnableStabilize());
    }

    private void FixedUpdate()
    {
        if (!canEvaluate || physicsEnabled)
        {
            return;
        }

        if (IsSupported())
        {
            unsupportedTimer = 0f;
            return;
        }

        unsupportedTimer += Time.fixedDeltaTime;
        if (unsupportedTimer >= unsupportedTimeToEnable)
        {
            EnablePhysics();
        }
    }


    //활성화 안정
    private IEnumerator CoEnableStabilize()
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        for (int i = 0; i < fixedFramesDelay; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        canEvaluate = true;
    }


    //물리 비활성화
    private void DisablePhysics()
    {
        wallRigidbody.isKinematic = true;
        wallRigidbody.useGravity = false;
        wallRigidbody.detectCollisions = false;

        wallRigidbody.linearVelocity = Vector3.zero;
        wallRigidbody.angularVelocity = Vector3.zero;
    }


    ///물리 활성화
    private void EnablePhysics()
    {
        wallRigidbody.isKinematic = false;
        wallRigidbody.useGravity = true;
        wallRigidbody.detectCollisions = true;

        wallRigidbody.linearVelocity = Vector3.zero;
        wallRigidbody.angularVelocity = Vector3.zero;

        physicsEnabled = true;
    }



    private bool IsSupported()
    {
        Bounds bound = wallCollider.bounds;
        Vector3 center = bound.center;
            
        //벽 지지
        Vector3[] dirs = { transform.right, -transform.right, transform.forward, -transform.forward };
        foreach (var d in dirs)
        {
            if (SphereCastSupported(center, bound, d, sideCheckDistance))
            {
                return true;
            }
        }
        return false;
    }

    private bool SphereCastSupported(Vector3 center, Bounds bound, Vector3 dir, float dist)
    {
        //시작점을 콜라이더 밖으로 조금 이동
        Vector3 origin = center;

        if (dir == Vector3.down)
        {
            origin = new Vector3(center.x, bound.min.y + checkRadius + 0.02f, center.z);
        }
        else
        {
            origin = center - dir.normalized * (checkRadius + 0.02f);
        }
            
        var hits = Physics.SphereCastAll(origin, checkRadius, dir, dist, supportMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.collider == null)
            {
                continue;
            }

            //자기 자신 제외
            if (hit.collider == wallCollider)
            {
                continue; 
            }

            //갈무리 트리거 제외
            if (ignoreTriggers && hit.collider.isTrigger)
            {
                continue;
            }    
            return true;
        }
        return false;
    }
}
