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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var gathered = other.GetComponent<CaptureObject>();
        if (gathered != null && currentTarget == gathered)
        {
            currentTarget = null;
        }
    }
}
