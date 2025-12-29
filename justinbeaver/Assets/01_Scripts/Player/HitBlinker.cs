using System.Collections;
using UnityEngine;

public class HitBlinker : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 0.08f; // 깜빡이는 간격
    [SerializeField] private bool useRendererEnabledToggle = true;

    private Renderer[] renderers;
    private Coroutine corutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    public void StartBlink(float duration)
    {
        StopBlink();
        corutine = StartCoroutine(CoBlink(duration));
    }

    public void StopBlink()
    {
        if (corutine != null)
        {
            StopCoroutine(corutine);
            corutine = null;
        }
        SetVisible(true);
    }

    private IEnumerator CoBlink(float duration)
    {
        float end = Time.time + duration;
        bool visible = true;

        while (Time.time < end)
        {
            visible = !visible;
            SetVisible(visible);
            yield return new WaitForSeconds(blinkInterval);
        }

        SetVisible(true);
        corutine = null;
    }

    private void SetVisible(bool visible)
    {
        if (renderers == null)
        {
            return;
        }

        if (useRendererEnabledToggle)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].enabled = visible;
                }
            }
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].enabled = visible;
                }    
            }
        }
    }
}