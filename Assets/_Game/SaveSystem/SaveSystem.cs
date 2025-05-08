using System.IO;
using UnityEngine;

/// <summary>
/// Reusable File Saving functions. Static class for easy access.
/// </summary>
public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/";
    public static readonly string FILE_NAME = "save.json";
    // save SaveData into file

    public static void SaveToFile(SaveData saveData)
    {
        Debug.Log("Save");
        // turn object into a json string
        string json = JsonUtility.ToJson(saveData);
        // write string to file
        File.WriteAllText(SAVE_FOLDER + FILE_NAME, json);
    }

    // load file into active SaveData and return it
    public static SaveData LoadFromFile()
    {
        Debug.Log("Load");
        SaveData saveData = new SaveData();
        // if we already have one, load it
        if (DoesSaveFileExist())
        {
            // turn file into a json string
            string json = File.ReadAllText(SAVE_FOLDER + FILE_NAME);
            // convert into object
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        // if not, create a new one and load that one
        else
        {
            saveData = CreateNewSaveFile();
        }

        return saveData;
    }

    public static SaveData CreateNewSaveFile()
    {
        SaveData saveData = new SaveData();
        SaveToFile(saveData);
        return saveData;
    }

    public static bool DoesSaveFileExist()
    {
        if (File.Exists(SAVE_FOLDER + FILE_NAME))
            return true;
        else
            return false;
    }
}
