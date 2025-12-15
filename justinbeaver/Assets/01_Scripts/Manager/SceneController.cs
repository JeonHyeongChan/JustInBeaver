using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum SceneType
{
    BeaverHouse,
    HumanHouse
}

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    //SceneType -> 실제 씬 이름
    private static readonly Dictionary<SceneType, string> SceneMap = 
        new Dictionary<SceneType, string>
        {
            { SceneType.BeaverHouse, "BeaverHouseScene" },
            { SceneType.HumanHouse, "HumanHouseScene" }
        };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// SceneType을 이용한 씬 로드
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        if (!SceneMap.TryGetValue(type, out string sceneName))
        {
            Debug.LogError($"{type} 해당 씬 없음");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    //추후 씬전환 연출 추가 가능
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);

        while (!oper.isDone)
        {
            yield return null;
        }
    }
}
