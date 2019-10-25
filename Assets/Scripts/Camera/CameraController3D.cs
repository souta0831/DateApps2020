using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController3D : MonoBehaviour
{    
    [SerializeField]
    private GameObject _targetObject = null;

    [SerializeField]
    private GameObject _lookTaget = null;

    [SerializeField]
    CameraController3DParameter _parameter;

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
        _nowfov = _parameter._fieldOfView;
        _nowPos = _targetObject.transform.position;

        CameraReset();
    }

    // Update is called once per frame
    void Update()
    {
        _parameter._rotAngle -= Input.GetAxis("Horizontal2") * _parameter._sensitivity;
        _parameter._heightAngle += Input.GetAxis("Vertical2") * _parameter._sensitivity;

        _parameter._heightAngle = Mathf.Clamp(_parameter._heightAngle, 0.0f, 80.0f);

        var delta = _targetObject.transform.position - _deltaTarget;
        _deltaTarget = _targetObject.transform.position;

        // 減衰
        if (_parameter._enableAtten)
        {
            var deltaPos = _targetObject.transform.position - _prevTargetPos;
            _prevTargetPos = _targetObject.transform.position;
            deltaPos *= _parameter._forwardDistance;

            _addForward += deltaPos * Time.deltaTime * 20.0f;
            _addForward = Vector3.Lerp(_addForward, Vector3.zero, Time.deltaTime * _parameter._attenRate);

            _nowPos = Vector3.Lerp(_nowPos, _targetObject.transform.position + transform.right* _parameter._width + Vector3.up * _parameter._height + _addForward, Mathf.Clamp(Time.deltaTime * _parameter._attenRate, 0.0f, 1.0f));
        }
        else _nowPos = _targetObject.transform.position + transform.right* _parameter._width + Vector3.up * _parameter. _height;

        // 手ブレ
        bool move = Mathf.Abs(delta.x) > 0.0f;
        var noise = new Vector3();
        if (_parameter._enableNoise)
        {
            var ns = (move ? _parameter._moveNoiseSpeed : _parameter._noiseSpeed);
            var nc = (move ? _parameter.moveNoiseCoeff : _parameter._noiseCoeff);

            var t = Time.time * ns;

            var nx = Mathf.PerlinNoise(t, t) * nc;
            var ny = Mathf.PerlinNoise(t + 10.0f, t + 10.0f) * nc;
            var nz = Mathf.PerlinNoise(t + 20.0f, t + 20.0f) * nc * 0.5f;
            noise = new Vector3(nx, ny, nz);
        }

        // FoV
        if (_parameter._enableFieldOfViewAtten) _nowfov = Mathf.Lerp(_nowfov, move ? _parameter._moveFieldOfView : _parameter._fieldOfView, Time.deltaTime);
        else _nowfov = _parameter._fieldOfView;
        _camera.fieldOfView = _nowfov;

        // カメラ位置
        if (_parameter._enableAtten) _nowRotAngle = Mathf.Lerp(_nowRotAngle, _parameter._rotAngle, Time.deltaTime * _parameter._rotAngleAttenRate);
        else _nowRotAngle = _parameter._rotAngle;

        if (_parameter._enableAtten) _nowHeightAngle = Mathf.Lerp(_nowHeightAngle, _parameter._heightAngle, Time.deltaTime * _parameter._rotAngleAttenRate);
        else _nowHeightAngle = _parameter._heightAngle;

        var deg = Mathf.PI / 180.0f;
        var cx = Mathf.Sin(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * _parameter._distance;
        var cz = -Mathf.Cos(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * _parameter._distance;
        var cy = Mathf.Sin(_nowHeightAngle * deg) * _parameter._distance;
        transform.position = _nowPos+ new Vector3(cx, cy, cz);

        // カメラ向き
        if (_lookTaget == null)
        {
            var rot = Quaternion.LookRotation((_nowPos - transform.position).normalized) * Quaternion.Euler(noise);
            if (_parameter._enableAtten) transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _parameter._angleAttenRate);
            else transform.rotation = rot;
        }
        else
        {
            transform.LookAt(_lookTaget.transform, Vector3.up);
        }
    }

   public void CameraReset()
    {
        _parameter._rotAngle = -_targetObject.transform.localEulerAngles.y;
        _parameter._heightAngle = 0.0f;
    }
}