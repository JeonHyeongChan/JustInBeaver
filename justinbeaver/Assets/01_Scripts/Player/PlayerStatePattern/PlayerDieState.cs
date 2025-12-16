using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private PlayerContext playerContext;
    private static readonly int HashDie = Animator.StringToHash("Die");


    public PlayerDieState (PlayerContext context)
    {
        playerContext = context;
    }


    public void Enter()
    {
        if (playerContext.playerController)
        {
            playerContext.playerController.enabled = false;
        }

        if (playerContext.playerRigid)
        {
            playerContext.playerRigid.linearVelocity = Vector3.zero;
            playerContext.playerRigid.isKinematic = true;
        }
        playerContext.playerAnimator.SetTrigger(HashDie); //애니메이션 트리거
    }

  
    public void Update() {}

    public void FixedUpdate() {}

    public void Exit() {}

}
