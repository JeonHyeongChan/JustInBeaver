using UnityEngine;
using UnityEngine.UI;

public class UI_GatherGauge : MonoBehaviour
{
    public Slider slider;

    public void SetValue(float v)
    {
        slider.value = Mathf.Clamp01(v);
    }
}