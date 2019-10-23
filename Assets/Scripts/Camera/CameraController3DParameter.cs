using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/CameraController3DParameter", fileName = "New CameraController3D Parameter")]

public class CameraController3DParameter : ScriptableObject
{
    [SerializeField, Range(1.0f, 5.0f)]
    public float _sensitivity = 3.0f;

    [SerializeField]
    public float _width = 1.0f;
    [SerializeField]
    public float _height = 1.5f;
    [SerializeField]
    public float _distance = 5.0f;
    [SerializeField]
    public float _heightAngle = 10.0f;
    [SerializeField]
    public float _rotAngle = 0.0f;
    [SerializeField]
    public float _rotAngleAttenRate = 5.0f;
    [SerializeField]
    public float _angleAttenRate = 40.0f;

    [SerializeField]
    public bool _enableAtten = true;
    [SerializeField]
    public float _attenRate = 3.0f;

    [SerializeField]
    public bool _enableNoise = true;
    [SerializeField]
    public float _noiseSpeed = 0.5f;
    [SerializeField]
    public float _moveNoiseSpeed = 1.5f;
    [SerializeField]
    public float _noiseCoeff = 1.3f;
    [SerializeField]
    public float moveNoiseCoeff = 2.5f;

    [SerializeField]
    public bool _enableFieldOfViewAtten = true;
    [SerializeField]
    public float _fieldOfView = 50.0f;
    [SerializeField]
    public float _moveFieldOfView = 60.0f;

    [SerializeField]
    public float _forwardDistance = 3.75f;

}
