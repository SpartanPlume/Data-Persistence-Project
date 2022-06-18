using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SyncFiles();

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
        public List<BestPlayData> BestPlays;

        public SaveData()
        {
            BestPlays = new List<BestPlayData>();
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
            // Destroy the old gameObject to not have a duplicate in the scene
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSaveData();

        // Initialize variables with the save data
        _playerNameInput.text = _saveData.LastPlayerName;
        _highScoreText.text = GetBestHighScore();
    }

    public void StartGame()
    {
        // Save the last player name entered for next startup
        _saveData.LastPlayerName = _playerNameInput.text;
        SaveSaveData();

        // Change scene to main scene (the actual game)
        SceneManager.LoadScene(1);
    }

    public void ShowHighscores()
    {
        // Change scene to highscores scene
        SceneManager.LoadScene(2);
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

        // WebGL bug: Writing in file is not instantaneous and must be done manually
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SyncFiles();
        }
    }

    public void AddNewBestPlay(int score)
    {
        SaveData.BestPlayData newBestPlay = new SaveData.BestPlayData();
        newBestPlay.PlayerName = _saveData.LastPlayerName;
        newBestPlay.Score = score;
        bool hasAddedNewBestPlay = false;
        for (int i = 0; i < _saveData.BestPlays.Count; ++i)
        {
            SaveData.BestPlayData bestPlay = _saveData.BestPlays[i];
            if (score > bestPlay.Score)
            {
                _saveData.BestPlays.Insert(i, newBestPlay);
                break;
            }
        }
        if (!hasAddedNewBestPlay)
        {
            _saveData.BestPlays.Add(newBestPlay);
        }
        if (_saveData.BestPlays.Count > 5)
        {
            _saveData.BestPlays.RemoveAt(5);
        }
        SaveSaveData();
    }

    public string GetBestHighScore()
    {
        SaveData.BestPlayData bestPlay = _saveData.BestPlays.Count > 0 ? _saveData.BestPlays[0] : new SaveData.BestPlayData();
        return $"Highscore: {bestPlay.PlayerName}: {bestPlay.Score}";
    }

    public string GetHighScores()
    {
        if (_saveData.BestPlays.Count == 0)
        {
            return "No highscore yet";
        }
        string highscoresText = "";
        for (int i = 0; i < _saveData.BestPlays.Count && i < 5; ++i)
        {
            SaveData.BestPlayData bestPlay = _saveData.BestPlays[i];
            highscoresText += $"{i + 1}. {bestPlay.PlayerName} - {bestPlay.Score}\n";
        }
        return highscoresText;
    }
}
