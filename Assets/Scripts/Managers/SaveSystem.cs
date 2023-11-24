using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static bool hasSaveData = false;

    public static void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData(player);
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/player.json";
        File.WriteAllText(path, json);
        hasSaveData = true;
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "player.json");
    }

    public static bool SaveExists()
    {
        string path = GetSavePath();
        return File.Exists(path);
    }

    public static bool HasSaveData()
    {
        return hasSaveData;
    }
}
