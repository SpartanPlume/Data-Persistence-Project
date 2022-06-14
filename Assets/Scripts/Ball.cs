using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _maxVelocity = 5.0f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision other)
    {
        Vector3 velocity = _rigidbody.velocity;

        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;

        //max velocity
        if (velocity.magnitude > _maxVelocity)
        {
            velocity = velocity.normalized * _maxVelocity;
        }

        _rigidbody.velocity = velocity;
    }
}
