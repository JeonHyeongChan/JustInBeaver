using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    [Header("이동/점프 설정")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.0f;
    public float groundOffset = 0.1f;

    [Header("회전 설정")]
    public float rotateSpeed = 10f;


    private Rigidbody rigid;
    private Vector2 moveInput;
    private bool isGrounded;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }



    private void FixedUpdate()
    {
        GroundCheck();
        Move();
    }


    private void Jump()
    {
        Vector3 vel = rigid.linearVelocity;
        vel.y = 0f;
        rigid.linearVelocity = vel;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }



    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        // 정규화
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        // 이동
        Vector3 currentVel = rigid.linearVelocity;
        Vector3 targetVel = move * moveSpeed;

        rigid.linearVelocity = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        
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


    private void GroundCheck()
    {
        Vector3 origin = transform.position + Vector3.up * groundOffset;
        isGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance);
    }



    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            Jump();
        }
    }
}
