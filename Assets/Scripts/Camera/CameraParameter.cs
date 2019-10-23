using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParameter : ScriptableObject
{
    [SerializeField, Range(1.0f, 5.0f)]
    private float _sensitivity = 3.0f;

    [SerializeField]
    private GameObject _targetObject;

    [SerializeField]
    private float _width = 1.0f;
    [SerializeField]
    private float _height = 1.5f;
    [SerializeField]
    private float _distance = 5.0f;
    [SerializeField]
    private float _heightAngle = 10.0f;
    [SerializeField]
    private float _rotAngle = 0.0f;
    [SerializeField]
    private float _rotAngleAttenRate = 5.0f;
    [SerializeField]
    private float _angleAttenRate = 40.0f;

    [SerializeField]
    private bool _enableAtten = true;
    [SerializeField]
    private float _attenRate = 3.0f;

    [SerializeField]
    private bool _enableNoise = true;
    [SerializeField]
    private float _noiseSpeed = 0.5f;
    [SerializeField]
    private float _moveNoiseSpeed = 1.5f;
    [SerializeField]
    private float _noiseCoeff = 1.3f;
    [SerializeField]
    private float moveNoiseCoeff = 2.5f;

    [SerializeField]
    private bool _enableFieldOfViewAtten = true;
    [SerializeField]
    private float _fieldOfView = 50.0f;
    [SerializeField]
    private float _moveFieldOfView = 60.0f;

    [SerializeField]
    private float _forwardDistance = 2.0f;
}
