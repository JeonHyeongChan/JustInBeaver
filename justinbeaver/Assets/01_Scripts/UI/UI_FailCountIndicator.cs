using UnityEngine;

public class UI_FailCountIndicator : MonoBehaviour
{
    [Header("Poclain")]
    [SerializeField] private RectTransform poclain; // 포크레인

    [Header("Log Positions (0~MaxFail)")]
    [SerializeField] private RectTransform[] logSlots;

    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            UpdateVisual(RuleManager.Instance.GetEscapeFailCount());
            RuleManager.Instance.OnEscapeFailedVisual += UpdateVisual;
            RuleManager.Instance.OnTotalResetRequired += ResetVisual;
        }
    }

    private void OnDisable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnEscapeFailedVisual -= UpdateVisual;
            RuleManager.Instance.OnTotalResetRequired -= ResetVisual;
        }
    }

    private void UpdateVisual(int failCount)
    {
        if (poclain == null || logSlots == null || logSlots.Length == 0)
            return;

        int index = Mathf.Clamp(failCount, 0, logSlots.Length - 1);

        poclain.position = logSlots[index].position;
    }

    private void ResetVisual()
    {
        UpdateVisual(0);
    }
}