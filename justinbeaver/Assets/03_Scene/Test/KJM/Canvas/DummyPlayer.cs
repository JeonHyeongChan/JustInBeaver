using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    float gatherValue = 0f;
    float gatherSpeed = 1f; // 초당 증가량

    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            gatherValue += Time.deltaTime * gatherSpeed;
            UIManager.Instance.gatherGauge.SetValue(gatherValue);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            gatherValue = 0f;
            UIManager.Instance.gatherGauge.ResetGauge();
        }
    }
}