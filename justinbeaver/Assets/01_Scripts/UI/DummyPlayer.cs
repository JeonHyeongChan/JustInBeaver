using UnityEngine;
using UnityEngine.InputSystem;

public class DummyPlayer : MonoBehaviour
{
    public UI_GatherGauge gatherGauge;
    float value = 0f;

    void Update()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            gatherGauge.gameObject.SetActive(true);

            value += Time.deltaTime;
            gatherGauge.SetValue(value);
        }
        else
        {
            value = 0f;
            gatherGauge.SetValue(0f);
            // 원하면 아래 줄 주석 해제
            // gatherGauge.gameObject.SetActive(false);
        }
    }
}