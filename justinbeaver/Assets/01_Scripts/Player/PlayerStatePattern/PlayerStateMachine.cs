using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public IPlayerState CurrentState { get; private set; }

    public void ChangeState(IPlayerState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }


    private void Start()
    {
        var playerContext = GetComponent<PlayerContext>();
        ChangeState(new PlayerNormalState(playerContext));
    }


    private void Update()
    {
        CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}
