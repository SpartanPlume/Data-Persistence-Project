using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private int _lineCount = 6;
    [SerializeField] private Rigidbody _ball;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private GameObject _gameOverText;

    private MenuManager _menuManager;
    private bool _hasStarted = false;
    private bool _isGameOver = false;
    private int _points;

    // Start is called before the first frame update
    private void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < _lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(_brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.OnDestroyed.AddListener(AddPoint);
            }
        }

        _menuManager = MenuManager.Instance;
        if (_menuManager)
        {
            _highScoreText.text = _menuManager.GetHighScore();
        }
    }

    private void Update()
    {
        if (!_hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _hasStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                _ball.transform.SetParent(null);
                _ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        _points += point;
        _scoreText.text = $"Your score : {_points}";
    }

    public void GameOver()
    {
        _isGameOver = true;
        _gameOverText.SetActive(true);
        if (_menuManager)
        {
            _menuManager.UpdateBestPlay(_points);
            _highScoreText.text = _menuManager.GetHighScore();
        }
    }
}
