﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;

public class Player : MonoBehaviour
{
   [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private float _jumpPower = 100.0f;
    [SerializeField]
    private float _sliding_speed = 6.0f;
    [SerializeField]
    private float _sliding_LR_speed = 2.0f;
    [SerializeField]
    private GameObject AttackCollider=null;
    [SerializeField]
    private GameObject Slash_Efect=null;
    //カメラ
    [SerializeField]
    private Camera Camera;
    private CameraController3D _camera_script;
    //スライディングした時に再生するパーティクル
    [SerializeField]
    private List<ParticleSystem> SlidingParticleList=new List<ParticleSystem>();

    private Rigidbody _rigidbody;
    private Animator _animator;
    private BoxCollider _box_collider;
    //保存用
    private GameObject _temp_slash_fx=null;
    private Vector3 _collider_size = Vector3.zero;
    private Vector3 _collider_center = Vector3.zero;
    //ステート
    private StateProcessor StateProcessor = new StateProcessor();
    private PlayerStateIdle StateIdle = new PlayerStateIdle();
    private PlayerStateRun StateRun = new PlayerStateRun();
    private PlayerStateSliding StateSliding = new PlayerStateSliding();
    private PlayerStateJump StateJump = new PlayerStateJump();
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _box_collider = GetComponent<BoxCollider>();
        _camera_script = Camera.GetComponent<CameraController3D>();
        _collider_size = _box_collider.size;
        _collider_center = _box_collider.center;
        StateProcessor.State = StateIdle;
        StateIdle.execDelegate = Idle;
        StateRun.execDelegate = Run;
        StateSliding.execDelegate = Sliding;

        SlidingParticleSwitch(false);


    }

    void Update()
    {
        State();
        if (IsGround()) {
            if (InputController.GetButtonDown(ButtonID.B))
            {
                _rigidbody.AddRelativeForce(transform.up * _jumpPower);

            }
        }
    }
    void State()
    {

        if (StateProcessor.State == null)
        {
            return;
        }
        StateProcessor.Execute();


    }
    private void Idle()
    {
        _animator.SetBool("is_running", false);
        _animator.SetBool("is_sliding", false);

        if (InputController.GetAxis(AxisID.L_Horizontal) != 0.0f || InputController.GetAxis(AxisID.L_Vertical) != 0.0f)
        {
            StateProcessor.State = StateRun;
        }


    }
    private void Run()
    {
        _animator.SetBool("is_running", true);
        var x = InputController.GetAxisRaw(AxisID.L_Horizontal);
        var z = InputController.GetAxisRaw(AxisID.L_Vertical);


        Vector3 camForward = Vector3.Scale(Camera.transform.forward, Vector3.right + Vector3.forward);
        Vector3 moveForward = (camForward * z) + (Camera.transform.right * x);

        Debug.Log(moveForward);

        if (moveForward.magnitude > 0.01f)
        {
            transform.position += moveForward * _speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        if (InputController.GetButtonDown(ButtonID.R1)) 
        {
            //スライディングに移行
            StateProcessor.State = StateSliding;
            SetSlidingCollider(true);
            SlidingParticleSwitch(true);


        }
        if (InputController.GetAxis(AxisID.L_Horizontal) == 0.0f && InputController.GetAxis(AxisID.L_Vertical) == 0.0f&&IsGround())
        {
            StateProcessor.State = StateIdle;
        }

    }
    private void Sliding()
    {
        _animator.SetBool("is_sliding", true);

        var x = InputController.GetAxisRaw(AxisID.L_Horizontal);

        var moveForward = Vector3.Scale(transform.forward, new Vector3(1.0f, 0.0f, 1.0f));
        var moveLR = (transform.right* x).normalized;
        transform.position += (moveForward * _sliding_speed+ moveLR *_sliding_LR_speed)*Time.deltaTime;

        if (InputController.GetButtonDown(ButtonID.Y))
        {

            _animator.SetTrigger("Attack");

        }

        if (!InputController.GetButton(ButtonID.R1) && !IsUpWallHit() && IsGround())
        {
            //アイドルに移行
            StateProcessor.State = StateIdle;
            SlidingParticleSwitch(false);
            SetSlidingCollider(false);


        }


    }
    private void Jump()
    {
    }
    //天井判定
    private bool IsUpWallHit()
    {
        Ray up_ray = new Ray(transform.position, transform.up);
        //天井判定の距離
        float tenjou = 2;
        int layermask = 1 << 9;

        if (Physics.Raycast(up_ray,tenjou, layermask))
        {
            return true;
        }
        return false;
    }
    //地面判定
    private bool IsGround()
    {
        Ray down_ray = new Ray(transform.position+new Vector3(0,0.5f,0), -transform.up);
        float gorund = 1;
        int layermask = 1 << 9;
        if (Physics.Raycast(down_ray, gorund, layermask))
        {
            Debug.Log("地面なう");
            return true;
            
        }

        return false;
    }
   
    //アニメーションイベントで呼び出します
    private void AtackEvent()
    {

        AttackCollider.SetActive(true);
    }
    private void AtackEndEvent()
    {
        AttackCollider.SetActive(false);

    }
    private void SlidingParticleSwitch(bool isPlay)
    {
        for (int i = 0; i < SlidingParticleList.Count; i++)
        {
            if (SlidingParticleList[i] == null) continue;

            if (isPlay)
            {
                SlidingParticleList[i].Play();
                continue;
            }
            SlidingParticleList[i].Stop();
        }


    }
    private void SetSlidingCollider(bool isSliding)
    {
        if (isSliding)
        {
            _box_collider.size = new Vector3(_collider_size.x, _collider_size.y / 2, _collider_size.z);
            _box_collider.center = new Vector3(_collider_center.x, _collider_center.y / 2, _collider_center.z);
            return;
        }
        _box_collider.size = _collider_size;
        _box_collider.center = _collider_center;
    }
    public PlayerStateID GetState()
    {
        return StateProcessor.State.GetState();
    }

}
