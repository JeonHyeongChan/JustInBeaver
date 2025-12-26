using UnityEngine;

public class HitboxTest: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
            
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null)
        {
            return;
        }

        health.TakeDamage();
    }
}
