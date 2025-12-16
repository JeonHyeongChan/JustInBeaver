public class PlayerNormalState : IPlayerState
{
    private PlayerContext playerContext;
    
    public PlayerNormalState(PlayerContext context)
    {
        playerContext = context;
    }

    public void Enter()
    {
        playerContext.playerController.enabled = true;
    }

    public void Update() {}

    public void FixedUpdate() {}

    public void Exit() {}
}
