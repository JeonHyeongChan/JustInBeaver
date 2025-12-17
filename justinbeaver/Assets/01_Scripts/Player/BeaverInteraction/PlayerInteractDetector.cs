using UnityEngine;

public class PlayerInteractDetector : MonoBehaviour
{
    public IInteractable currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (currentTarget == interactable)
                currentTarget = null;
        }
    }
}
