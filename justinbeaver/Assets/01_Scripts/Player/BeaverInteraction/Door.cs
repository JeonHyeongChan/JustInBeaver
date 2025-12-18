using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneName;  // 문오브젝트에 이동할 씬 이름 입력
    [SerializeField] private Transform uiAnchor;

    public string InteractText => $"[Z] GO {sceneName}";
    public Transform UIAnchor => uiAnchor ? uiAnchor : transform;

    public void Interact(PlayerController player)
    {
        SceneManager.LoadScene(sceneName);
    }
}