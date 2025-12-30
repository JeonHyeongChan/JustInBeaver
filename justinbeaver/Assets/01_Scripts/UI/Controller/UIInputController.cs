using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInputController : MonoBehaviour
{
    [Header("Submit Debounce")]
    [SerializeField] private float submitCooldown = 0.2f;
    private float nextSubmitTime;

    GameObject seletedObject;

    [Header("Navigate")]
    [SerializeField] private float axisThreshold = 0.5f;
    [SerializeField] private float repeatDelay = 0.18f;
    private float nextRepeatTime;

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        seletedObject = EventSystem.current.currentSelectedGameObject;

        if (!ctx.performed)
        {
            return;
        }

        //같은 입력 연타/홀드로 인한 2번 클릭 방지
        if (Time.unscaledTime < nextSubmitTime)
        {
            return;
        }

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

        var esc = EventSystem.current;
        if (esc == null)
        {
            return;
        }

        var selected = esc.currentSelectedGameObject;
        if (selected == null)
        {
            return;
        }

        var button = selected.GetComponent<Button>();
        if (button != null && button.interactable)
        {
            nextSubmitTime = Time.unscaledTime + submitCooldown;
            button.onClick.Invoke();
            return;
        }
    }
}
