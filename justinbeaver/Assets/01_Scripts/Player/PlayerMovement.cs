using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;

    private PlayerSpeedBuff speedBuff;

    void Awake()
    {
        speedBuff = GetComponent<PlayerSpeedBuff>();
    }

    void Update()
    {
        float penalty = PlayerStatsManager.Instance.GetSpeedPenalty();
        float speedAfterWeight = Mathf.Max(1f, baseSpeed - penalty);

        float buffMultiplier = speedBuff != null
            ? speedBuff.currentMulitiplier
            : 1f;

        float finalSpeed = speedAfterWeight * buffMultiplier;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        transform.Translate(dir * finalSpeed * Time.deltaTime);
    }
}