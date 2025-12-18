using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string InteractText => throw new System.NotImplementedException();

    public Transform UIAnchor => throw new System.NotImplementedException();

    public bool RequiresHold => throw new System.NotImplementedException();

    public float HoldDuration => throw new System.NotImplementedException();

    public void Interact(PlayerController player)
    {
        Debug.Log("NPC with talking");
        //상호작용 필요한 것들
    }

    public void OnHoldCancel(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        throw new System.NotImplementedException();
    }
}
