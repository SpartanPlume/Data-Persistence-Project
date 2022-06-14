using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _fastTypeSpeed = 5.0f;
    [SerializeField] private float _maxMovement = 2.0f;

    [SerializeField] private List<GameObject> _paddleTypes;
    private int _currentPaddleTypeIndex;

    // Update is called once per frame
    private void Update()
    {
        MovePaddle();
        ChangePaddle();
    }

    private void MovePaddle()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        float currentSpeed = _currentPaddleTypeIndex == 3 ? _fastTypeSpeed : _speed;
        pos.x += input * currentSpeed * Time.deltaTime;

        if (pos.x > _maxMovement)
            pos.x = _maxMovement;
        else if (pos.x < -_maxMovement)
            pos.x = -_maxMovement;

        transform.position = pos;
    }

    private void ChangePaddle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _paddleTypes[_currentPaddleTypeIndex].SetActive(false);
            _currentPaddleTypeIndex = (_paddleTypes.Count + _currentPaddleTypeIndex - 1) % _paddleTypes.Count;
            _paddleTypes[_currentPaddleTypeIndex].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            _paddleTypes[_currentPaddleTypeIndex].SetActive(false);
            _currentPaddleTypeIndex = (_currentPaddleTypeIndex + 1) % _paddleTypes.Count;
            _paddleTypes[_currentPaddleTypeIndex].SetActive(true);
        }
    }
}
