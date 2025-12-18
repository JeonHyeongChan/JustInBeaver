using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneName;  // 문오브젝트에 이동할 씬 이름 입력
    [SerializeField] private Transform uiAnchor;

    [Header("Escape Setting")]
    [SerializeField] private string escapeSceneName = "BeaverHouseScene";
    [SerializeField] private float escapeHoldTime = 2f;

    public bool RequiresHold => sceneName == escapeSceneName; // 씬이름 확인
    public float HoldDuration => RequiresHold ? escapeHoldTime : 0f;

    public string InteractText => RequiresHold
        ? $"[Z] Hold to Go {sceneName}": $"[Z] GO {sceneName}";
    public Transform UIAnchor => uiAnchor ? uiAnchor : transform;    

    public void Interact(PlayerController player)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        UIManager.Instance?.ShowGatherGauge();
        UIManager.Instance?.GatherGauge?.SetValue(progress01);
    }

    public void OnHoldCancel(PlayerController player)
    {
        UIManager.Instance?.HideGahterGauge();
        UIManager.Instance?.GatherGauge?.SetValue(0f);
    }
}