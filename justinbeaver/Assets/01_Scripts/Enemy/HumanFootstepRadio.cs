using UnityEngine;
using UnityEngine.AI;

public class HumanFootstepRadio : MonoBehaviour
{
    public float triggerDistance; // 감지 거리
    public SFXType footstepSFX = SFXType.HumanWalk; // 사운드

    private Transform beaver;
    private EnemyStatePattern human;
    private NavMeshAgent agent;

    private void Awake()
    {
        human = GetComponent<EnemyStatePattern>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (human != null)
            beaver = human.PlayerTrf;
    }

    private void OnDisable()
    {
        SoundManager.Instance?.StopHumanFootstep(transform);
    }

    private void Update()
    {
        if (beaver == null || agent == null || human == null)
            return;

        float distance = Vector3.Distance(transform.position, beaver.position);

        bool shouldPlay =
            distance <= triggerDistance && agent.velocity.sqrMagnitude > 0.1f &&
            (human.IsInChaseOrAlert());

        if (shouldPlay)
        {
            SoundManager.Instance?.PlayHumanFootstep(transform, footstepSFX);
        }
        else
        {
            SoundManager.Instance?.StopHumanFootstep(transform);
        }
    }
}