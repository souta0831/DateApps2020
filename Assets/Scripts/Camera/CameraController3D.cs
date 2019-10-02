using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController3D : MonoBehaviour
{
    [SerializeField,Range(1.0f,5.0f)]
    private float _sensitivity =3.0f;

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

    private Camera _camera;
    private Vector3 _addForward;
    private Vector3 _deltaTarget;
    private Vector3 _nowPos;
    private float _nowfov;

    private float _nowRotAngle;
    private float _nowHeightAngle;

    private Vector3 _prevTargetPos;

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        _nowfov = _fieldOfView;
        _nowPos = _targetObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _rotAngle -= Input.GetAxis("Horizontal2") * _sensitivity;
        _heightAngle += Input.GetAxis("Vertical2") * _sensitivity;

        _heightAngle = Mathf.Clamp(_heightAngle, 0.0f, 80.0f);

        var delta = _targetObject.transform.position - _deltaTarget;
        _deltaTarget = _targetObject.transform.position;

        // 減衰
        if (_enableAtten)
        {
            var deltaPos = _targetObject.transform.position - _prevTargetPos;
            _prevTargetPos = _targetObject.transform.position;
            deltaPos *= _forwardDistance;

            _addForward += deltaPos * Time.deltaTime * 20.0f;
            _addForward = Vector3.Lerp(_addForward, Vector3.zero, Time.deltaTime * _attenRate);

            _nowPos = Vector3.Lerp(_nowPos, _targetObject.transform.position + transform.right*_width + Vector3.up * _height + _addForward, Mathf.Clamp(Time.deltaTime * _attenRate, 0.0f, 1.0f));
        }
        else _nowPos = _targetObject.transform.position + transform.right* _width + Vector3.up * _height;

        // 手ブレ
        bool move = Mathf.Abs(delta.x) > 0.0f;
        var noise = new Vector3();
        if (_enableNoise)
        {
            var ns = (move ? _moveNoiseSpeed : _noiseSpeed);
            var nc = (move ? moveNoiseCoeff : _noiseCoeff);

            var t = Time.time * ns;

            var nx = Mathf.PerlinNoise(t, t) * nc;
            var ny = Mathf.PerlinNoise(t + 10.0f, t + 10.0f) * nc;
            var nz = Mathf.PerlinNoise(t + 20.0f, t + 20.0f) * nc * 0.5f;
            noise = new Vector3(nx, ny, nz);
        }

        // FoV
        if (_enableFieldOfViewAtten) _nowfov = Mathf.Lerp(_nowfov, move ? _moveFieldOfView : _fieldOfView, Time.deltaTime);
        else _nowfov = _fieldOfView;
        _camera.fieldOfView = _nowfov;

        // カメラ位置
        if (_enableAtten) _nowRotAngle = Mathf.Lerp(_nowRotAngle, _rotAngle, Time.deltaTime * _rotAngleAttenRate);
        else _nowRotAngle = _rotAngle;

        if (_enableAtten) _nowHeightAngle = Mathf.Lerp(_nowHeightAngle, _heightAngle, Time.deltaTime * _rotAngleAttenRate);
        else _nowHeightAngle = _heightAngle;

        var deg = Mathf.PI / 180.0f;
        var cx = Mathf.Sin(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * _distance;
        var cz = -Mathf.Cos(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * _distance;
        var cy = Mathf.Sin(_nowHeightAngle * deg) * _distance;
        transform.position = _nowPos+ new Vector3(cx, cy, cz);

        // カメラ向き
        var rot = Quaternion.LookRotation((_nowPos - transform.position).normalized) * Quaternion.Euler(noise);
        if (_enableAtten) transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _angleAttenRate);
        else transform.rotation = rot;

        //Z注目
        if (Input.GetButtonDown("Fire1"))
        {
            _rotAngle = -_targetObject.transform.localEulerAngles.y;
            _heightAngle = 0.0f;
        }
    }
}