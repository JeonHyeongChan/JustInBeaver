using UnityEngine;

public class BootstrapLoader : MonoBehaviour
{
    private void Start()
    {
        SceneController.Instance.LoadScene(SceneType.Title);
    }
}
