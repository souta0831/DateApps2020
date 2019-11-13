using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController3D : MonoBehaviour
{    
    [SerializeField]
    private GameObject m_targetObject = default;

    [SerializeField]
    CameraController3DParameter m_parameter = default;

    [SerializeField]
    private Player_LockOn m_player_LockOn = default;

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
        _nowfov = m_parameter._fieldOfView;
        _nowPos = m_targetObject.transform.position;

        CameraReset();
    }

    // Update is called once per frame
    void Update()
    {
        m_parameter._rotAngle -= Input.GetAxis("Horizontal2") * m_parameter._sensitivity;
        m_parameter._heightAngle += Input.GetAxis("Vertical2") * m_parameter._sensitivity;

        m_parameter._heightAngle = Mathf.Clamp(m_parameter._heightAngle, 0.0f, 80.0f);

        var delta = m_targetObject.transform.position - _deltaTarget;
        _deltaTarget = m_targetObject.transform.position;

        // 減衰
        if (m_parameter._enableAtten)
        {
            var deltaPos = m_targetObject.transform.position - _prevTargetPos;
            _prevTargetPos = m_targetObject.transform.position;
            deltaPos *= m_parameter._forwardDistance;

            _addForward += deltaPos * Time.deltaTime * 20.0f;
            _addForward = Vector3.Lerp(_addForward, Vector3.zero, Time.deltaTime * m_parameter._attenRate);

            _nowPos = Vector3.Lerp(_nowPos, m_targetObject.transform.position + transform.right* m_parameter._width + Vector3.up * m_parameter._height + _addForward, Mathf.Clamp(Time.deltaTime * m_parameter._attenRate, 0.0f, 1.0f));
        }
        else _nowPos = m_targetObject.transform.position + transform.right* m_parameter._width + Vector3.up * m_parameter. _height;

        // FoV
        bool move = Mathf.Abs(delta.x) > 0.0f;
        if (m_parameter._enableFieldOfViewAtten) _nowfov = Mathf.Lerp(_nowfov, move ? m_parameter._moveFieldOfView : m_parameter._fieldOfView, Time.deltaTime);
        else _nowfov = m_parameter._fieldOfView;
        _camera.fieldOfView = _nowfov;

        // カメラ位置
        if (m_parameter._enableAtten) _nowRotAngle = Mathf.Lerp(_nowRotAngle, m_parameter._rotAngle, Time.deltaTime * m_parameter._rotAngleAttenRate);
        else _nowRotAngle = m_parameter._rotAngle;

        if (m_parameter._enableAtten) _nowHeightAngle = Mathf.Lerp(_nowHeightAngle, m_parameter._heightAngle, Time.deltaTime * m_parameter._rotAngleAttenRate);
        else _nowHeightAngle = m_parameter._heightAngle;

        var deg = Mathf.PI / 180.0f;
        var cx = Mathf.Sin(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * m_parameter._distance;
        var cz = -Mathf.Cos(_nowRotAngle * deg) * Mathf.Cos(_nowHeightAngle * deg) * m_parameter._distance;
        var cy = Mathf.Sin(_nowHeightAngle * deg) * m_parameter._distance;
        transform.position = _nowPos+ new Vector3(cx, cy, cz);

        // カメラ向き
        if (m_player_LockOn.NowLockOnGameObject() == null)
        {
            var rot = Quaternion.LookRotation((_nowPos - transform.position).normalized);
            if (m_parameter._enableAtten) transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * m_parameter._angleAttenRate);
            else transform.rotation = rot;
        }
        else
        {        
            transform.LookAt(Vector3.Lerp(m_targetObject.transform.position, m_player_LockOn.NowLockOnGameObject().transform.position, 0.5f), Vector3.up);        
        }

        //角度制限
        float angle_x = 180f <= transform.eulerAngles.x ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;

        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angle_x, 0.0f, 60.0f),
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }

   public void CameraReset()
    {
        m_parameter._rotAngle = - m_targetObject.transform.localEulerAngles.y;
        m_parameter._heightAngle = 0.0f;
    }

}