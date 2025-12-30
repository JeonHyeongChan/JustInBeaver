using UnityEngine;
using System.IO;

/// <summary>
/// 게임 저장 / 불러오기
/// 에디터: JSON으로, 빌드: AES 암호화로
/// </summary>
public static class SaveManager
{
    //저장 파일 경로
    // Application.persistentDataPath:
    // Windows: AppData/LocalLow/폴더명/게임명
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "jib_save.dat");

    /// <summary>
    /// 저장
    /// </summary>
    /// <param name="data"></param>
    public static void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true); // JSON 문자열로 변환

#if UNITY_EDITOR
            // 에디터에서는 사람이 읽을 수 있게 JSON 그대로 저장
            File.WriteAllText(SavePath, json);
#else
            // 빌드에서는 암호화 저장
            File.WriteAllText(SavePath, CryptoUtil.Encrypt(json));
#endif

            Debug.Log("Save Success");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save Failed: {e.Message}");
        }
    }

    /// <summary>
    /// 불러오기
    /// </summary>
    /// <returns></returns>
    public static SaveData Load()
    {
        if (!File.Exists(SavePath)) // 없으면 새게임
            return null;

        try
        {
            string data = File.ReadAllText(SavePath); // 문자열 읽어옴

#if UNITY_EDITOR
            return JsonUtility.FromJson<SaveData>(data);
#else
            string json = CryptoUtil.Decrypt(data);
            return JsonUtility.FromJson<SaveData>(json);
#endif
        }
        catch
        {
            Debug.LogWarning("Save file corrupted. Deleting.");

            Delete(); // 파일에 문제 있으면 삭제 후 새게임으로 유도
            return null;
        }
    }

    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    public static void Delete()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}

#region
//public static class SaveManager
//{
//    private static string SavePath =>
//        Path.Combine(Application.persistentDataPath, "save.json");

//    public static void Save(SaveData data)
//    {
//        string json = JsonUtility.ToJson(data, true);
//        File.WriteAllText(SavePath, json);
//        Debug.Log($"[SaveManager] Saved\n{json}");
//    }

//    public static SaveData Load()
//    {
//        if (!File.Exists(SavePath))
//            return null;

//        return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
//    }

//    public static bool HasSave()
//    {
//        return File.Exists(SavePath);
//    }

//    public static void Delete()
//    {
//        if (File.Exists(SavePath))
//            File.Delete(SavePath);
//    }
//}
#endregion