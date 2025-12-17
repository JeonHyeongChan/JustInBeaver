using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        Debug.Log("NPC with talking");
        //상호작용 필요한 것들
    }
}
