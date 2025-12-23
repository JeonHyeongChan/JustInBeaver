using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyStatePattern : MonoBehaviour
{
    private IEnemyState currentState;

    public Transform player;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    private Animator animator;
    public NavMeshAgent agent;
    public float attackCooldown;
    public int attackDamage;
    bool isAttacking = false;
    public float updateInterval;
    private float timeSinceLastUpdate;
    public float beaverSpottedTime;
    private float beaverSpottedLastUpdate;
    public GameObject hitBox;
    public LightView lightView;
    public float movementRange;
    public GameObject flashLight;

    Vector3 alertTargetPos;
    Vector3 lightInEnemy;
    private PlayerGatherSignal gatherSignal;

    public float MoveSpeed => moveSpeed;
    public float DetectRange => detectRange;
    public Transform PlayerTrf => player;
    public Animator Animator => animator;
    public float AttackRange => attackRange;
    public NavMeshAgent Agent => agent;
    public float UpdateInterval => updateInterval;
    public float TimeSinceLastUpdate => timeSinceLastUpdate;
    public float BeaverSpottedTime => beaverSpottedTime;
    public float BeaverSpottedLastUpdate => beaverSpottedLastUpdate;
    public Vector3 AlertTargetPos => alertTargetPos;

    private bool isAttackAnimPlaying = false;
    public bool IsAttackAnimPlaying => isAttackAnimPlaying;
    public LightView LightView => lightView;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        lightView = GetComponent<LightView>();
    }

    private void OnDestroy()
    {
        currentState = null;
    }

    private void Start()
    {
        BindPlayer();   // 시작시 Player 바인딩
        
        SetState(new SleepState(this));
        hitBox.SetActive(false);
    }
    private void Update()
    {
        //if (GameManager.Instance.State != GameState.Playing) return;
        if (currentState == null) return;
        currentState.Update();


    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        BindPlayer();   //씬 로드 및 활성화 시 재바인딩

        //if (player == null) return;
        //gatherSignal = player.GetComponent<PlayerGatherSignal>();
        //if (gatherSignal == null) return;
        //gatherSignal.OnGatherStart += HandleGatherStart;
        //gatherSignal.OnGatherEnd += HandleGatherEnd;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (gatherSignal == null) return;
        gatherSignal.OnGatherStart -= HandleGatherStart;
        gatherSignal.OnGatherEnd -= HandleGatherEnd;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindPlayer();   // 씬 이동시 Player 재탐색
    }

    /// <summary>
    /// Player 바인딩(Tag로)
    /// </summary>
    private void BindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            return;
        }
             
        player = playerObject.transform;

        //gatherSignal 연결
        gatherSignal = player.GetComponent<PlayerGatherSignal>();
        if (gatherSignal != null)
        {
            Debug.Log("연결 완료");
            gatherSignal.OnGatherStart += HandleGatherStart;
            gatherSignal.OnGatherEnd += HandleGatherEnd;
        }

    }

    private void HandleGatherStart(Vector3 anchorPos)
    {
        Debug.Log("핸들개터스타트");
        if (currentState is SleepState || currentState is IdleState)
        {
            Debug.Log("갈무리 if");
            SetAlertTargetPos(anchorPos);
            SetState(new AlertState(this));
        }
    }
    private void HandleGatherEnd()
    {
        // 갈무리 끝났다고 즉시 돌아갈지/계속 추격
        // 일단 여기서는 아무 것도 안 함
        // SetState(new ChaseState(this)) 하기
    }
    private IEnumerator WakeThenAlert()  //무한 보류 
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        animator.SetBool("isSleeping", false);
        animator.SetBool("isRunning", true);

        animator.Update(0f);
        yield return null;

        SetState(new AlertState(this));
    }
    public void SetAlertTargetPos(Vector3 pos)
    {
        alertTargetPos = pos;
    }
    public void LightViewTargetPos(Vector3 pos)
    {
       lightInEnemy = pos;
    }
    IEnumerator AttackDelay()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;
        while (isAttacking)
        {
            Attack(attackDamage);
            yield return new WaitForSeconds(animLength);
        }
    }

    IEnumerator AttackAnimLock()
    {
        // Animator 반영 대기
        yield return null;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        if (animLength < 0.1f)
            animLength = 0.3f; // 안전장치

        yield return new WaitForSeconds(animLength);

        isAttackAnimPlaying = false;
    }
    IEnumerator HitboxWindow()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;
        // 킥이 실제로 맞는 프레임까지 약간 대기
        yield return new WaitForSeconds(0.1f);

        hitBox.SetActive(true);

        // 판정 유지 시간
        yield return new WaitForSeconds(0.15f);

        hitBox.SetActive(false);
    }
    public void StartAttackCooldown()
    {
        if (isAttacking) { return; }
        isAttacking = true;
        StartCoroutine("AttackDelay");
    }
    public void StopAttackCooldown()
    {
        if (!isAttacking) { return; }
        isAttacking = false;
        StopCoroutine("AttackDelay");
    }

    void Attack(int Damage)
    {
        if (currentState is IdleState) return;
        if (isAttackAnimPlaying) return;
        isAttackAnimPlaying = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.ResetTrigger("isKicking");
        animator.SetTrigger("isKicking");
        //플레이어 데미지 처리
        
        StartCoroutine(AttackAnimLock());
        StartCoroutine(HitboxWindow());
    }

    public void Sleep()
    {
        animator.SetBool("isSleeping", true);
    }
 
    public Vector3 GetRandomPositionNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movementRange;
        randomDirection += transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, movementRange, NavMesh.AllAreas))
        {
            return navHit.position;
        }
        else
        {
            return transform.position;
        }
    }

    public void LightViewOn()
    {
        lightView.enabled = true;
    }
    public void LightViewOff()
    {
        lightView.enabled = false;
    }

    public void WakeUp()
    {
        agent.isStopped = false;
        SetState(new AlertState(this));
    }

    public void SetState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
