using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Button upgradeButton;
    public Button ExitButton;

    private float enableDelay = 0.15f;
    private bool locked;


    private IEnumerator CoEnableButtons()
    {
        //시간 기준 딜레이
        yield return new WaitForSecondsRealtime(enableDelay);

        locked = false;
        if (upgradeButton)
        {
            upgradeButton.interactable = true;
        }
        if (ExitButton)
        {
            ExitButton.interactable = true;
        }
    }


    void Start()
    {
        upgradeButton.onClick.AddListener(OnClickUpgradeButton);

        ExitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnEnable()
    {
        locked = true;

        StartCoroutine(CoEnableButtons());
    }

    public void OnClickUpgradeButton()
    {
        if (locked)
        {
            return;
        }
        locked = true;
        UIManager.Instance.ShowUpgradeUI();
    }

    public void OnClickExitButton()
    {
        if (locked)
        {
            return;
        }
        locked = true;
        UIManager.Instance.HideShopUI();
    }
}
