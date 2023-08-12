using System.IO;
using UnityEngine;

public class DataManager
{
    private Data _gameData;
    private static readonly string _dataFilePath = Path.Combine(Application.persistentDataPath, "GameData.json");

    public DataManager()
    {
        _gameData = new Data();
    }

    public void SetBestScore(int score)
    {
        if (_gameData == null)
        {
            _gameData = new Data();
        }

        _gameData.score = score;
    }

    public Data GetGameData()
    {
        return _gameData;
    }

    public void Save()
    {
        using StreamWriter writer = new StreamWriter(_dataFilePath);
        string dataToWrite = JsonUtility.ToJson(_gameData);
        writer.Write(dataToWrite);
    }

    public void Load()
    {
        if(File.Exists(_dataFilePath) == false)
        {
            return;
        }

        using StreamReader reader = new StreamReader(_dataFilePath);
        string dataToLoad = reader.ReadToEnd();
        _gameData = JsonUtility.FromJson<Data>(dataToLoad);
    }

    [System.Serializable]
    public class Data
    {
        public int score = 0;
    }
}
