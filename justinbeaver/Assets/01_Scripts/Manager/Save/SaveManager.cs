using UnityEngine;
using System.IO;

/// <summary>
/// 게임 저장 / 불러오기
/// Editor: JSON, Build: AES 암호화
/// </summary>
public static class SaveManager
{
    // 저장 파일 경로
    // Application.persistentDataPath:
    // Windows: AppData/LocalLow/폴더명/게임명
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "jib_save.dat");

    /// <summary>
    /// 저장
    /// </summary>
    public static void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true); // JSON 문자열 변환

#if UNITY_EDITOR
            File.WriteAllText(SavePath, json);
#else
            string encrypted = CryptoUtil.Encrypt(json);
            File.WriteAllText(SavePath, encrypted);
#endif
            Debug.Log($"[SaveManager] Save Success → {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager] Save Failed: {e.Message}");
        }
    }

    /// <summary>
    /// 불러오기
    /// </summary>
    public static SaveData Load()
    {
        if (!File.Exists(SavePath))
            return null;

        try
        {
            string raw = File.ReadAllText(SavePath); // 문자열 읽어오기

#if UNITY_EDITOR
            return JsonUtility.FromJson<SaveData>(raw);
#else
            string json = CryptoUtil.Decrypt(raw);
            return JsonUtility.FromJson<SaveData>(json);
#endif
        }
        catch
        {
            Debug.LogWarning("[SaveManager] Save file corrupted. Deleting.");
            Delete(); // 문자 파일 삭제 후 새게임 유도
            return null;
        }
    }

    public static bool HasSave() => File.Exists(SavePath);

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