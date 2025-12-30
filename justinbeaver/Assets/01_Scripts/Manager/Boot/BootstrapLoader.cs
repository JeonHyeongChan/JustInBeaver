using UnityEngine;

public class BootstrapLoader : MonoBehaviour
{
    private void Start()
    {
        Debug.Assert(SceneController.Instance != null);
        Debug.Assert(ItemManager.Instance != null);
        Debug.Assert(StorageManager.Instance != null);
        Debug.Assert(GameManager.Instance != null);

        SceneController.Instance.LoadScene(SceneType.Title);
    }
}
