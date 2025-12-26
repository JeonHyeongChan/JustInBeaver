using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractDetector : MonoBehaviour
{
    public IInteractable currentTarget;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentTarget = null; // 씬이 바뀔때 이전 상호작용 대상 제거 해야함
        UIManager.Instance?.HideInteractHint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentTarget = interactable;

            UIManager.Instance?.ShowInteractHint(interactable.UIAnchor, interactable.InteractText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (currentTarget == interactable)
            {
                currentTarget = null;

                UIManager.Instance?.HideInteractHint();
            }
        }
    }
}
