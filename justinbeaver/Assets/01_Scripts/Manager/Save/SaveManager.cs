using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] Saved\n{json}");
    }

    public static SaveData Load()
    {
        if (!File.Exists(SavePath))
            return null;

        return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
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
