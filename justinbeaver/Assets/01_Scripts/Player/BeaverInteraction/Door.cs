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
        ? $"[Z]키를 유지하여 {sceneName}으로 이동" : $"[Z]키를 눌러 {sceneName}으로 이동";
    public Transform UIAnchor => uiAnchor ? uiAnchor : transform;

    public void Interact(PlayerController player)
    {
        player.SetInputLocked(false);
        UIManager.Instance?.HideGahterGauge();
        UIManager.Instance?.GatherGauge?.SetValue(0f);


        //탈출구
        if (sceneName == escapeSceneName)
        {
            FindAnyObjectByType<StorageTransfer>(FindObjectsInactive.Include)   //인벤토리속 내용물 창고로 옮기기ㅣ
          ?.TransferInventoryStorage();
     
            RuleManager.Instance?.OnEscapeSuccess();
            return;
        }

        //일반문
        SceneManager.LoadScene(sceneName);
    }

    public void OnHoldUpdate(PlayerController player, float progress01)
    {
        player.SetInputLocked(true);
        UIManager.Instance?.ShowGatherGauge();
        UIManager.Instance?.GatherGauge?.SetValue(progress01);
    }

    public void OnHoldCancel(PlayerController player)
    {
        player.SetInputLocked(false);
        UIManager.Instance?.HideGahterGauge();
        UIManager.Instance?.GatherGauge?.SetValue(0f);
    }
}