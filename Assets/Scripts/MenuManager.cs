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
        public class BestPlayData
        {
            public string PlayerName;
            public int Score;
        }

        public string LastPlayerName;
        public BestPlayData BestPlay;
    }

    public MenuManager Instance;
    private SaveData _saveData;

    [SerializeField] private TMP_InputField _playerNameInput;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance)
        {
            // Destroy the new gameObject to not have a duplicate in the scene
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSaveData();
        }
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

            // Initialize variables with the save data
            _playerNameInput.text = _saveData.LastPlayerName;
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
}
