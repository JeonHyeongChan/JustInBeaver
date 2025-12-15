using UnityEngine;
using UnityEngine.UI;

public class UI_GatherGauge : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider.value = 0f;
        slider.interactable = false;
    }

    public void SetValue(float value)
    {
        slider.value = Mathf.Clamp01(value);
    }

    public void ResetGauge()
    {
        slider.value = 0f;
    }
}