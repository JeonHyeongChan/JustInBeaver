using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (UIManager.Instance != null &&
            UIManager.Instance.gameFailUI != null &&
            UIManager.Instance.gameFailUI.activeSelf)
        {
            UIManager.Instance.ConfirmGameFail();
        }
    }
}
