using UnityEngine;

public class BeaverFootstep : MonoBehaviour
{
    [Header("Step Timing")]
    [SerializeField] private float minInterval = 0.15f; // 최고 속도일 때
    [SerializeField] private float maxInterval = 0.6f; // 가방 무거울 때
    [SerializeField] private float maxSpeedRef = 6f; // 기본 속도

    private float stepTimer;
    private Rigidbody rigid;
    private PlayerController controller;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!IsMovingOnGround())
        {
            stepTimer = 0f;
            return;
        }
        
        float speed = GetSpeed();

        float temp = Mathf.Clamp01(speed / maxSpeedRef); // 0 ~ 1

        float interval = Mathf.Lerp(maxInterval, minInterval, temp); // 빠를수록 인터벌 감소

        stepTimer += Time.deltaTime;

        if (stepTimer >= interval)
        {            
            SoundManager.Instance?.PlaySFX(SFXType.BeaverMove);
            stepTimer = 0f;
        }
    }

    private bool IsMovingOnGround()
    {        
        return controller.IsGrounded && !controller.IsRolling && GetSpeed() > 0.2f;
    }

    private float GetSpeed()
    {
        Vector3 velocity = rigid.linearVelocity;
        velocity.y = 0f;
        return velocity.magnitude;
    }
}