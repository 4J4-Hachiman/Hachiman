using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    public GameData()
    {
        playerPosition = new Vector3(150, 0, 35);
    }
}