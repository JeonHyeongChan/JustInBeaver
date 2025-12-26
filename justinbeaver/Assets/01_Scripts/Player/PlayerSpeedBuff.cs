using System.Collections;
using UnityEngine;

public class PlayerSpeedBuff : MonoBehaviour
{
    [SerializeField] private float hitSpeedMulitiplier = 1.5f;
    [SerializeField] private float hitSpeedDuration = 3f;

    private Coroutine coroutine;
    private float multitiplier = 1f;
    public float currentMulitiplier => multitiplier;

    public void ApplyHitSpeedBoot()
    {
        Debug.Log("[SpeedBuff] ApplyHitSpeedBoot");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(CoBoost());
        
    }

    private IEnumerator CoBoost()
    {
        multitiplier = hitSpeedMulitiplier;
        Debug.Log($"[SpeedBuff] BOOST ON");

        yield return new WaitForSeconds(hitSpeedDuration);

        multitiplier = 1f;
        Debug.Log("[SpeedBuff] BOOST OFF");
        coroutine = null;
    }
}
