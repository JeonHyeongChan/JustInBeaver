using UnityEngine;

public class DontDestroyOnLoadRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
