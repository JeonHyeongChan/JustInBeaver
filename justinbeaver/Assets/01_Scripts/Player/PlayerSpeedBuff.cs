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
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(CoBoost());
        
    }

    private IEnumerator CoBoost()
    {
        multitiplier = hitSpeedMulitiplier;

        yield return new WaitForSeconds(hitSpeedDuration);

        multitiplier = 1f;
        coroutine = null;
    }
}
