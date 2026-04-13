using UnityEngine;

public class PlayerGatherDetector : MonoBehaviour
{
    public CaptureObject currentTarget {  get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        var gather = other.GetComponent<CaptureObject>();
        if (gather != null)
        {
            currentTarget = gather;

            // Show interact hint when entering gatherable object's trigger
            if (UIManager.Instance != null)
            {
                // If the target is locked due to home level, show its locked message
                if (!gather.IsUnlocked())
                {
                    UIManager.Instance.ShowInteractHint(gather.transform, gather.GetLockedMessage());
                }
                else if (!gather.isCompleted && gather.gameObject.activeInHierarchy)
                {
                    UIManager.Instance.ShowInteractHint(gather.transform, "[X]키를 눌러 갈무리하기");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var gathered = other.GetComponent<CaptureObject>();
        if (gathered != null && currentTarget == gathered)
        {
            currentTarget = null;

            // Hide hint when leaving
            UIManager.Instance?.HideInteractHint();
        }
    }
}
