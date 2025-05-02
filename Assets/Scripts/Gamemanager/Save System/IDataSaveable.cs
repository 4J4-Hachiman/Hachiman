using UnityEngine;

public interface IDataSaveable
{
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}
