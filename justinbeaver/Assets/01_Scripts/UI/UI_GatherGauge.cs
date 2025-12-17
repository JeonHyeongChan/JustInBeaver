using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_GatherGauge : MonoBehaviour
{
    [Header("UI 세팅")]
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text TMPtext;
    [HideInInspector] CanvasGroup canvasGroup;

    [Header("옵션")]
    public bool hideOnAwake = true;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>(true);
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;
            slider.value = 0f;
        }

        if (hideOnAwake)
        {
            HideImmediate();
        }
    }


    public void SetValue(float value01)
    {
        float value = Mathf.Clamp01(value01);

        if (slider != null)
        {
            slider.value = value;
        }

        if (TMPtext != null)
        {
            TMPtext.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HideImmediate()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false);
    }
}
