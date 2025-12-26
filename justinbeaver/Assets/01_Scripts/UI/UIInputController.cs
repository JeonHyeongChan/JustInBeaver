using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
   GameObject SelectedObject;

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

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            Debug.Log("OnSubmit1");
            return;
        }
        //if (SelectedObject.name == "Exit")
        //{
        //    UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickExitButton();
        //}
        Debug.Log("OnSubmit2");
       //if (UIManager.Instance.shopUI.activeSelf && UIManager.Instance.gameFailUI == null && UIManager.Instance.gameSuccessUI == null && UIManager.Instance.shopUI != null)
       //{
       //    SelectedObject = EventSystem.current.currentSelectedGameObject;
       //    if (SelectedObject.name == "Exit")
       //    {
       //        UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickExitButton();
       //        return;
       //    }
       //    else if (SelectedObject.name == "Upgrade")
       //    {
       //        UIManager.Instance.shopUI.GetComponent<UI_Shop>().OnClickUpgradeButton();
       //        return;
       //    }
       //        //Debug.LogWarning("쇼샵 비활성화");
       //    return;
       //}
        //if (UIManager.Instance.gameFailUI == null && UIManager.Instance.gameSuccessUI == null && UIManager.Instance.shopUI != null)
        //{
        //    UIManager.Instance.ShowShopUI();
        //    Debug.LogWarning("쇼샵 활성화");
        //    return;
        //
        //}



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
