using UnityEngine;

public class BeaverFootstep : MonoBehaviour
{
    public float stepInterval = 0.2f;

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

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            Debug.Log("Beaver Footstep!");
            SoundManager.Instance?.PlaySFX(SFXType.BeaverMove);
            stepTimer = 0f;
        }
    }

    private bool IsMovingOnGround()
    {
        Vector3 velocity = rigid.linearVelocity;
        velocity.y = 0f;

        return velocity.magnitude > 0.2f && controller.IsGrounded && !controller.IsRolling;
    }
}