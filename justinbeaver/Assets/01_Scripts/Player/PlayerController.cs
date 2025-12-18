using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("이동/점프 설정")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.0f;
    public float groundOffset = 0.1f;

    [Header("구르기 설정")]
    public float rollSpeed = 10f;        //구르기 시 속도
    public float rollDuration = 0.35f;   //구르기 지속 시간
    public float rollCooldown = 0.6f;    //구르기 쿨타임

    [Header("회전 설정")]
    public float rotateSpeed = 10f;

    [Header("애니메이터")]
    public Animator animator;

    private Rigidbody rigid;
    private Vector2 moveInput;
    private bool isGrounded;

    private bool jumpLocked;            //점프 후 착지 전까지 재점프 금지
    private bool wasGrounded;           //착지 판정용

    private bool isRolling;
    private float rollEndTime;
    private float nextRollTime;
    private Vector3 rollDir;            //구르기 방향

    //상호작용 홀드
    private float interactHoldTimer;
    private bool isHoldingInteract;
    private IInteractable holdingTarget;



    //Animator 파라미터 해시
    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashIsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int HashJump = Animator.StringToHash("Jump");
    private static readonly int HashRoll = Animator.StringToHash("Roll");



    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        HandleInteractHold();
    }

    private void FixedUpdate()
    {
        GroundCheck();

        //착지 판정
        if (!wasGrounded && isGrounded)
        {
            jumpLocked = false;
        }
        wasGrounded = isGrounded;

        if (isRolling) RollUpdate();
        else Move();

        UpdateAnimator();
    }


    private void Jump()
    {

        //점프 시작 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger(HashJump);
        }

        Vector3 vel = rigid.linearVelocity;
        vel.y = 0f;
        rigid.linearVelocity = vel;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpLocked = true; //점프 소비(착지 전까지 재점프 금지)
    }


    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        //정규화
        if (move.sqrMagnitude > 1f)
        {
            move.Normalize();
        }
            
        //이동
        Vector3 currentVel = rigid.linearVelocity;
        Vector3 targetVel = move * moveSpeed;

        rigid.linearVelocity = new Vector3(targetVel.x, currentVel.y, targetVel.z);


        //회전(입력이 있을 때만)
        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void StartRoll(Vector3 desiredDir)
    {
        if (Time.time < nextRollTime)
        {
            return;     //쿨타임
        }    

        if (isRolling)
        {
            return;
        }    


        //방향이 없다면 현재 바라보는 방향으로 구르기
        if (desiredDir.sqrMagnitude < 0.001f)
        {
            desiredDir = transform.forward;
        }
        desiredDir.y = 0f;
        desiredDir.Normalize();

        isRolling = true;
        rollDir = desiredDir;
        rollEndTime = Time.time + rollDuration;
        nextRollTime = Time.time + rollCooldown;


        //롤 애니메이션 트리거
        if (animator != null)
        {
            animator.SetTrigger(HashRoll);
        }
    }

    private void RollUpdate()
    {
        //구르는 동안: xz는 rollDir로 고정, y는 물리 유지
        Vector3 currentVel = rigid.linearVelocity;
        Vector3 rollVel = rollDir * rollSpeed;
        rigid.linearVelocity = new Vector3(rollVel.x, currentVel.y, rollVel.z);


        //구르는 동안 방향 고정 회전
        Quaternion targetRot = Quaternion.LookRotation(rollDir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.fixedDeltaTime
        );

        if (Time.time >= rollEndTime)
        {
            isRolling = false;
        }
    }

    private void GroundCheck()
    {
        Vector3 origin = transform.position + Vector3.up * groundOffset;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance);
    }

    private void UpdateAnimator()
    {
        if (animator == null)
        {
            return;
        }
        //실제 물리 속도 기준으로 Speed 계산
        Vector3 horizontalVel = rigid.linearVelocity;
        horizontalVel.y = 0f;
        float speed = horizontalVel.magnitude;

        animator.SetFloat(HashSpeed, speed);
        animator.SetBool(HashIsGrounded, isGrounded);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            return;
        }

        if (isRolling)
        {
            return;
        }

        if (isGrounded && !jumpLocked)
        {
            Jump();
        }
    }

    public void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            return;
        }
        if (!isGrounded)
        {
            return;
        }

        //입력 방향으로 굴러가게(입력 없으면 forward)
        Vector3 desired = new Vector3(moveInput.x, 0f, moveInput.y);
        StartRoll(desired);
    }

    public void OnGather(InputAction.CallbackContext ctx)
    {
        var context = GetComponent<PlayerContext>();
        if (context == null)
        {
            return;
        } 
            

        //누르기 시작
        if (ctx.started)
        {
            context.isGatherHolding = true;

            var target = context.playerGatherDetector != null ? context.playerGatherDetector.currentTarget : null;
            if (target == null) 
            {
                return;
            }

            //이미 완료/비활성 대상이면 시작 금지
            if (!target.gameObject.activeInHierarchy || target.isCompleted)
            {
                return;
            }


            //시간 내 재입력 + 같은 대상이면 이어하기
            float startProgress = 0f;
            bool canResume =
                context.lastGatherTarget == target &&
                (Time.time - context.lastGatherCancelTime) <= context.gatherResumeWindow;

            startProgress = canResume ? context.lastGatherProgress : target.progress;

            //타겟 진행도에 시작값 반영
            target.SetProgress(startProgress);

            context.playerStateMachine.ChangeState(new PlayerGatherState(context, target, startProgress));
            return;
        }

        //누르기 종료
        if (ctx.canceled)
        {
            context.isGatherHolding = false;
            return;
        }
    }

    /// <summary>
    /// 상호작용 Z키
    /// </summary>
    /// <param name="ctx"></param>
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        var detector = GetComponent<PlayerInteractDetector>();
        if (detector == null)
            return;

        //홀드 시작
        if (ctx.started)
        {
            holdingTarget = detector.currentTarget;
            if (holdingTarget == null)
                return;

            interactHoldTimer = 0f;
            isHoldingInteract = true;

            //즉시 상호작용 대상이면 바로
            if (!holdingTarget.RequiresHold)
            {
                ExecuteInteract(holdingTarget);
            }

            return;
        }

        //홀드 취소
        if (ctx.canceled)
        {
            CancelHold();
        }
    }

    private void HandleInteractHold()
    {
        if (!isHoldingInteract || holdingTarget == null)
            return;

        if (!holdingTarget.RequiresHold)
            return;
        
        interactHoldTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(interactHoldTimer / holdingTarget.HoldDuration);

        holdingTarget.OnHoldUpdate(this, progress);

        if (progress >= 1f)
        {
            ExecuteInteract(holdingTarget);
        }
    }

    private void CancelHold()
    {
        if (holdingTarget != null)
        {
            holdingTarget.OnHoldCancel(this);
        }

        isHoldingInteract = false;
        interactHoldTimer = 0f;
        holdingTarget = null;
    }

    private void ExecuteInteract(IInteractable target)
    {
        isHoldingInteract = false;
        interactHoldTimer = 0f;

        UIManager.Instance?.HideInteractHint();

        target.Interact(this);
        holdingTarget = null;
    }
}
