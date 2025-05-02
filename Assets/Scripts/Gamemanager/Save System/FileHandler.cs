using UnityEngine;
using System.IO;

public class SaveFileHandler
{   
    private string directPath = "";
    private string fileName = "";

    public SaveFileHandler(string directPath, string fileName)
    {
        this.directPath = directPath;
        this.fileName = fileName;
    } 

    public GameData LoadGameData()
    {
        string fullPath = Path.Combine(directPath, fileName);
        GameData deserializedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string readFile;
                using FileStream stream = new (fullPath, FileMode.Open); 
                {
                    using StreamReader reader = new (stream); 
                    {
                        readFile = reader.ReadToEnd();
                    }
                }

                deserializedData = JsonUtility.FromJson<GameData>(readFile);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"The file at {fullPath} cant be found \n {e}");
                throw;
            }
        }
        return deserializedData;
    }

    public void SaveGameData(GameData gameData)
    {
        string fullPath = Path.Combine(directPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string serializedData = JsonUtility.ToJson(gameData, true);

            using FileStream stream = new (fullPath, FileMode.Create);
            {
                using StreamWriter writer = new (stream);
                {
                    writer.Write(serializedData);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Cant find the file at {fullPath} \n {e}");
            throw;
        }
    }
}