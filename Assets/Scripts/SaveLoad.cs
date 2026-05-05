using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad main;

    SaveFile activeSaveFile;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            Load();
        }
        else
        {
            activeSaveFile = new SaveFile();
        }
    }

    public void Save()
    {
        activeSaveFile.runLogs = RunLogger.main.AllLogs();

        string json = JsonUtility.ToJson(activeSaveFile);

        File.WriteAllText(Application.dataPath + "/save.txt", json);
        Debug.Log("<color=green> SAVED! </color> " + json);
    }

    public void Load()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");

        activeSaveFile = JsonUtility.FromJson<SaveFile>(saveString);

        RunLogger.main.LoadRuns(activeSaveFile.runLogs);
    }

    public void WipeSaveFile()
    {
        File.Delete(Application.dataPath + "/save.txt");
        activeSaveFile = new SaveFile();
    }
}

public class SaveFile
{
    public RunLog[] runLogs;
}