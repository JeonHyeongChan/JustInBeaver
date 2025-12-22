using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        //실패 UI
        if (UIManager.Instance != null &&
            UIManager.Instance.gameFailUI != null &&
            UIManager.Instance.gameFailUI.activeSelf)
        {
            UIManager.Instance.ConfirmGameFail();
            return;
        }

        //성공 UI
        if (UIManager.Instance != null &&
            UIManager.Instance.gameSuccessUI != null &&
            UIManager.Instance.gameSuccessUI.activeSelf)
        {
            UIManager.Instance.ConfirmEscapeSuccess();
            return;
        }
    }
}
