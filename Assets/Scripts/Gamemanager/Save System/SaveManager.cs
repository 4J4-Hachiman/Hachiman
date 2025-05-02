using UnityEngine;

public class SaveManager
{
    public static SaveManager Instance { get;  private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A SaveManager instance already exists");
        }

        Instance = this;
    }

    public void LoadGame(GameData data)
    {

    }

    public void SaveGame(ref GameData data)
    {

    }
}
