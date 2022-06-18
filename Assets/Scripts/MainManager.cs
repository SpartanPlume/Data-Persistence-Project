using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private int _lineCount = 6;

    [SerializeField] private GameObject _paddle;
    [SerializeField] private Rigidbody _ball;
    [SerializeField] private float _initialBallVelocity = 1.5f;
    [SerializeField] private float _ballVelocityIncrement = 0.5f;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highScoreText;
    [SerializeField] private GameObject _gameOverText;

    private MenuManager _menuManager;
    private bool _hasStarted = false;
    private bool _isGameOver = false;
    private int _points;

    public int BrickCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        _menuManager = MenuManager.Instance;
        if (_menuManager)
        {
            _highScoreText.text = _menuManager.GetBestHighScore();
        }
    }

    private void Update()
    {
        if (BrickCount <= 0)
        {
            StartLevel();
        }

        if (!_hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _hasStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                _ball.transform.SetParent(null);
                _ball.AddForce(forceDir * _initialBallVelocity, ForceMode.VelocityChange);
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

    private void StartLevel()
    {
        _hasStarted = false;
        _initialBallVelocity += _ballVelocityIncrement;

        // Reset Ball to initial position
        _ball.velocity = Vector3.zero;
        _ball.transform.position = _paddle.transform.position + new Vector3(0.0f, 0.15f, 0.0f);
        _ball.transform.SetParent(_paddle.transform);

        // Create brick objects to destroy
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < _lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Brick brick = Instantiate(_brickPrefab, position, Quaternion.identity);
                brick.MainManager = this;
                brick.PointValue = pointCountArray[i];
                brick.OnDestroyed.AddListener(AddPoint);
            }
        }
        BrickCount = _lineCount * perLine;
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
            _menuManager.AddNewBestPlay(_points);
            _highScoreText.text = _menuManager.GetBestHighScore();
        }
    }
}
