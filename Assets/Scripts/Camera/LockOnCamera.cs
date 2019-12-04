using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    [SerializeField]
    private const float ANGLE_LIMIT_UP = 60f;
    [SerializeField]
    private const float ANGLE_LIMIT_DOWN = 0f;

    [SerializeField]
    private Transform m_playerTransform = default;

    [SerializeField]
    private Player_LockOn m_player_LockOn = default;

    [SerializeField]
    private float m_rotateSpeed = 3.0f;

    private Camera m_camera = default;

    private float AttenRate = 5.0f; // 減衰比率

    public float FoVAttenRate = 2.0f; // FoVの減衰比率
    private float MovedFoV = 60.0f; // プレイヤーが移動している時のFoV
    private float FoV = 50.0f; // プレイヤーが立ち止まっている時のFoV

    private Vector3 prevPlayerPos; // 前フレームのプレイヤーの位置

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponentInChildren<Camera>();
        prevPlayerPos = m_playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Lerp補完
        var pos = m_playerTransform.position; // 本来到達しているべきカメラ位置
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * AttenRate); // Lerp減衰

        //FoV
        var moved = m_playerTransform.position != prevPlayerPos;
        prevPlayerPos = m_playerTransform.position;

        var fov = moved ? MovedFoV : FoV;
        m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, fov, Time.deltaTime * FoVAttenRate);

        //注目
        if (m_player_LockOn.NowLockOnGameObject()!=null)
        {
            LockAtTargetObject(m_player_LockOn.NowLockOnGameObject());
        }
        else
        {
            RotateCameraAngle();
        }

        //角度制限
        float angle_x = 180f <= transform.eulerAngles.x ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;

        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angle_x, ANGLE_LIMIT_DOWN, ANGLE_LIMIT_UP),
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }

    private void RotateCameraAngle()
    {
        Vector2 angle = new Vector2(
           Input.GetAxis("Horizontal2") * m_rotateSpeed,
           Input.GetAxis("Vertical2") * m_rotateSpeed
        );
        transform.eulerAngles += new Vector3(angle.y, angle.x);
    }

    private void LockAtTargetObject(GameObject target)
    {
        Vector3 _targetPos = target.transform.position;
        _targetPos.y = m_playerTransform.position.y;

        Vector3 relativePos = _targetPos - m_playerTransform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(relativePos, Vector3.up),0.25f);
    }
}
