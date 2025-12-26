using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            Debug.Log("OnSubmit1");
            return;
        }

        Debug.Log("OnSubmit2");
        if (UIManager.Instance.shopUI.activeSelf)
        {

            Debug.LogWarning("쇼샵 비활성화");
            UIManager.Instance.ShowUpgradeUI();
            return;
        }
        if (UIManager.Instance.gameFailUI == null && UIManager.Instance.gameSuccessUI == null && UIManager.Instance.shopUI != null)
        {
            UIManager.Instance.ShowShopUI();
            Debug.LogWarning("쇼샵 활성화");
            return;
          
        }
     
        Debug.Log(UIManager.Instance != null);
        //Debug.Log(UIManager.Instance.gameFailUI != null);
        //Debug.Log(UIManager.Instance.gameFailUI.activeSelf);


        //실패 UI
        if (UIManager.Instance != null &&
            UIManager.Instance.gameFailUI != null &&
            UIManager.Instance.gameFailUI.activeSelf )
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
