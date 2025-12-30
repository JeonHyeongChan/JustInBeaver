using UnityEngine;

public class EndingNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private EndingCutscene endingCutscene;

    public string InteractText => "[Z] Watch Ending";
    public Transform UIAnchor => transform;

    public bool RequiresHold => false;
    public float HoldDuration => 0f;

    private void OnEnable()
    {
        TryBindCutscene();
    }

    private void TryBindCutscene()
    {
        if (endingCutscene != null)
            return;

        endingCutscene = FindAnyObjectByType<EndingCutscene>();

        if (endingCutscene == null)
        {
            Debug.LogWarning("[EndingNPC] EndingCutscene not found yet. Will retry on interact.");
        }
        else
        {
            Debug.Log("[EndingNPC] EndingCutscene bound successfully");
        }
    }

    public void Interact(PlayerController player)
    {
        Debug.Log("EndingNPC Interact CALLED");

        if (RuleManager.Instance == null)
            return;

        Debug.Log("GameState = " + RuleManager.Instance.GetGameState());

        if (RuleManager.Instance.GetGameState() != GameState.EndingReady)
            return;

        if (endingCutscene == null) // 한번더 확인
        {
            TryBindCutscene();
        }

        if (endingCutscene == null)
        {
            Debug.LogError("EndingCutscene not assigned");
            return;
        }

        endingCutscene.PlayEnding();
    }

    public void OnHoldCancel(PlayerController player)
    {
        
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {        

    }
}
