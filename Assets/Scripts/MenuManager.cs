using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    private class SaveData
    {
        [System.Serializable]
        public class BestPlayData
        {
            public string PlayerName;
            public int Score;
        }

        public string LastPlayerName;
        public BestPlayData BestPlay;

        public SaveData()
        {
            BestPlay = new BestPlayData();
        }
    }

    public static MenuManager Instance;
    private SaveData _saveData;

    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance)
        {
            // Destroy the new gameObject to not have a duplicate in the scene
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSaveData();

        // Initialize variables with the save data
        _playerNameInput.text = _saveData.LastPlayerName;
        _highScoreText.text = GetHighScore();
    }

    public void StartGame()
    {
        // Save the last player name entered for next startup
        _saveData.LastPlayerName = _playerNameInput.text;
        SaveSaveData();

        // Change scene to main scene (the actual game)
        SceneManager.LoadScene(1);
    }

    private void LoadSaveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // Create a new SaveData if there is no file to load
            _saveData = new SaveData();
        }
    }

    private void SaveSaveData()
    {
        string json = JsonUtility.ToJson(_saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void UpdateBestPlay(int score)
    {
        if (score > _saveData.BestPlay.Score)
        {
            _saveData.BestPlay.PlayerName = _saveData.LastPlayerName;
            _saveData.BestPlay.Score = score;
            SaveSaveData();
        }
    }

    public string GetHighScore()
    {
        return $"Highscore: {_saveData.BestPlay.PlayerName}: {_saveData.BestPlay.Score}";
    }
}
