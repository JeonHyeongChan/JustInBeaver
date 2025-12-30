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
            button.onClick.Invoke();
            return;
        }

        // if (!IsAnyUIOpen())
        //     return;
        //

        //private void Update()
        //{
        //    if (GameObject.Find("Canvas") != null)
        //    {
        //        EventSystem.current.SetSelectedGameObject(GameObject.Find);
        //    }
        //      
        //        //SelectedObject = EventSystem.current.currentSelectedGameObject;
        //        Debug.Log(SelectedObject.name);s
        //    
        //}

        //if (SelectedObject.name == "Exit")
        //{
        //    UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickExitButton();
        //}

        //Debug.Log("OnSubmit2");
        //if (UIManager.Instance.shopUI.activeSelf)
        //{
        //    if (UIManager.Instance.shopUI.activeSelf && UIManager.Instance.gameFailUI == null && UIManager.Instance.gameSuccessUI == null && UIManager.Instance.shopUI != null)
        //    {
        //        SelectedObject = EventSystem.current.currentSelectedGameObject;
        //        if (SelectedObject.name == "Exit")
        //        {
        //            UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickExitButton();
        //            return;
        //        }
        //        else if (SelectedObject.name == "Upgrade")
        //        {
        //            UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickUpgradeButton();
        //            return;
        //        }
        //            //Debug.LogWarning("쇼샵 비활성화");
        //        return;
        //    }
        //    if (UIManager.Instance.gameFailUI == null && UIManager.Instance.gameSuccessUI == null && UIManager.Instance.shopUI != null)
        //    {
        //        UIManager.Instance.ShowShopUI();
        //        Debug.LogWarning("쇼샵 활성화");
        //        return;
        //    
        //    }
        //}
    }

    //private bool IsAnyUIOpen()
    //{
    //    if (UIManager.Instance == null) return false;
    //
    //    return (UIManager.Instance.shopUI != null && UIManager.Instance.shopUI.activeSelf)
    //   || (UIManager.Instance.upgradeUI != null && UIManager.Instance.upgradeUI.activeSelf)
    //   || (UIManager.Instance.gameFailUI != null && UIManager.Instance.gameFailUI.activeSelf)
    //   || (UIManager.Instance.gameSuccessUI != null && UIManager.Instance.gameSuccessUI.activeSelf);
    //}

}
