using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoresManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scores;

    // Start is called before the first frame update
    private void Start()
    {
        if (!MenuManager.Instance)
        {
            return;
        }

        _scores.text = MenuManager.Instance.GetHighScores();
    }

    public void BackToMainMenu()
    {
        // Change scene to menu scene
        SceneManager.LoadScene(0);
    }
}
