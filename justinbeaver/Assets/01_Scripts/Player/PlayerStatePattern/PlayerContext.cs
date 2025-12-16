using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [Header("참조")]
    public PlayerController playerController;
    public Animator playerAnimator;
    public Rigidbody playerRigid;
    public PlayerHealth playerhealth;
    public PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        if (!playerRigid)
        {
            playerRigid = GetComponent<Rigidbody>();
        }    
            
        if (!playerController)
        {
            playerController = GetComponent<PlayerController>();
        }

        if (!playerAnimator)
        {
            playerAnimator = GetComponentInChildren<Animator>();
        }

        if (!playerStateMachine)
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }    
            
        if (!playerhealth)
        {
            playerhealth = GetComponent<PlayerHealth>();
        }            
    }
}
