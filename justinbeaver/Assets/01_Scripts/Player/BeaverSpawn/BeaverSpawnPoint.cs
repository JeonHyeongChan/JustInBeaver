using UnityEngine;

public class BeaverSpawnPoint : MonoBehaviour
{
    public static Transform Current;

    private void Awake()
    {
        Current = transform;
    }
}
