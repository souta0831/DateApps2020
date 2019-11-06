﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerParameter _parameter;

    [SerializeField]
    private LifePointBase _lifePoint = null;

    [SerializeField]
    private Player_EffectManager _effectManager = null;

    [SerializeField]
    private GameObject AttackCollider = null;

    //カメラ
    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private EnemyManager  _enemyManager;

    //Thisコンポーネント
    private Rigidbody _rigidbody;
    private Animator _animator;
    private BoxCollider _box_collider;
    private LockonCursor _lockonCursor = null;

    //保存用
    private Vector3 _collider_size = Vector3.zero;
    private Vector3 _collider_center = Vector3.zero;
    private Vector3 _move_power = Vector3.zero;
    private Vector3 _buffer_pos = Vector3.zero;
    private Vector3 _nomal_vector = Vector3.zero;
    private Vector3 _move_direction = Vector3.zero;

    private float _stick_x = 0.0f;
    private float _stick_z = 0.0f;

    //ステート
    private StateProcessor StateProcessor = new StateProcessor();
    private PlayerStateIdle StateIdle = new PlayerStateIdle();
    private PlayerStateRun StateRun = new PlayerStateRun();
    private PlayerStateSliding StateSliding = new PlayerStateSliding();
    private PlayerStateJump StateJump = new PlayerStateJump();
    private PlayerStateWallRun StateWalRun = new PlayerStateWallRun();

    void Start()
    {
        //GetComponent
        Camera = Camera.main;
       _rigidbody = GetComponent<Rigidbody>();
       _animator = GetComponent<Animator>();
       _box_collider = GetComponent<BoxCollider>();
       _lockonCursor = GetComponent<LockonCursor>();

        //コライダーの大きさを取得
        _collider_size = _box_collider.size;
        _collider_center = _box_collider.center;

        //初期ステートセット
        StateProcessor.State = StateIdle;

        //イデレーターにセット
        StateIdle.execDelegate = IdleState;
        StateRun.execDelegate = RunState;
        StateSliding.execDelegate = SlidingState;
        StateWalRun.execDelegate = WallRunState;
        StateJump.execDelegate = JumpState;

        //LifePointセット
        _lifePoint.MaxPointSet(_parameter.MaxHp);
        _lifePoint.PointSet(_parameter.MaxHp);

        //Effect停止
        _effectManager.AllParticleStop();
    }

    void Update()
    {
        State();
        StickUpdate();
        Jump();      
        Move();
    }
    //-------------------------------------------------
    // 移動の関数のやつ？
    //-------------------------------------------------
    private void Move()
    {
       this.transform.position += _move_power;
        _buffer_pos = transform.position;
    }

    private void Jump()
    {
        if (InputController.GetButtonDown(Button.B) && IsGround())
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddRelativeForce(transform.up *  _parameter.JumpPower);
            StateProcessor.State = StateJump;
        }
    }

    private void StickUpdate()
    {
        _stick_x = Input.GetAxis("Horizontal");
        _stick_z = Input.GetAxis("Vertical");
    }

    //-------------------------------------------------
    // ステートのやつ
    //-------------------------------------------------
    private void State()
    {
        if (StateProcessor.State == null)
        {
            return;
        }
        StateProcessor.Execute();
    }
    private void IdleState()
    {
        _animator.SetBool("is_running", false);
        _animator.SetBool("is_sliding", false);

        if (InputController.GetAxisDown(Axis.L_Horizontal) != 0.0f || InputController.GetAxisDown(Axis.L_Vertical) != 0.0f)
        {
            StateProcessor.State = StateRun;
        }
        _move_power *= 0.9f;

    }
    private void RunState()
    {
        if (_stick_x == 0.0f && _stick_z == 0.0f)
        {
            Debug.Log("ステート移行");
            StateProcessor.State = StateIdle;
            return;
        }

        Vector2 _stickRange = new Vector2(_stick_x,_stick_z);

        _animator.SetBool("is_running", true);

        Vector3 camForward = Vector3.Scale(Camera.transform.forward, Vector3.right + Vector3.forward);
        Vector3 moveForward = (camForward * _stick_z) + (Camera.transform.right * _stick_x);
        //_move_power =  _parameter.RunSpeed * moveForward * Time.deltaTime;

        _move_power = moveForward.normalized * (_parameter.RunSpeed / Mathf.Sqrt(2.0f) * Time.deltaTime);

        this.transform.rotation = Quaternion.LookRotation(moveForward);

        if (InputController.GetButtonDown(Button.R1))
        {
            //スライディングに移行
            StateProcessor.State = StateSliding;
            SetSlidingCollider(true);
            _effectManager.AllParticlePlay();
        }

    }
    private void SlidingState()
    {
        _animator.SetBool("is_sliding", true);

        var moveForward = Vector3.Scale(transform.forward, Vector3.right + Vector3.forward);
        var moveLR = (transform.right * _stick_x);//.normalized;

        //_move_power = (moveForward * _parameter.SlidingSpeed + moveLR * _parameter.SlidingLRSpeed) * Time.deltaTime;
        _move_power += (moveLR * _parameter.SlidingLRSpeed) /10.0f* Time.deltaTime;
        if (_move_power.magnitude>= _parameter.SlidingLRSpeed)
        {
            _move_power = _move_power.normalized * _parameter.SlidingLRSpeed;
        }
        this.transform.position += moveForward * _parameter.SlidingSpeed * Time.deltaTime;
        _move_power.x *= 0.99f;

        if (InputController.GetButtonDown(Button.Y))
        {
            _animator.SetTrigger("Attack");
        }

        if (!InputController.GetButtonStay(Button.R1) && !IsUpWallHit() && IsGround())
        {
            //アイドルに移行
            StateProcessor.State = StateIdle;
            _effectManager.AllParticleStop();
            SetSlidingCollider(false);
        }
    }
    private void JumpState()
    {
        _effectManager.AllParticleStop();

        if (WallRunStartCheck())
        {
            Debug.Log("壁に当たった");
            StateProcessor.State = StateWalRun;
        }
        if (IsGround())
        {
            StateProcessor.State = StateIdle;
        }
    }
    private void WallRunState()
    {
        Vector3 WallRunVector = _move_direction + Vector3.Scale(((-_move_direction + _nomal_vector).normalized), _nomal_vector);
        _move_power = WallRunVector * _parameter.WallRunSpeed;
    }
    //-------------------------------------------------
    // 判定関数
    //-------------------------------------------------
    private bool WallRunStartCheck()
    {
        _move_direction = (transform.position - _buffer_pos).normalized;

        Ray ray = new Ray(transform.position, _move_direction);

        if (!Physics.Raycast(ray, out RaycastHit hit, _parameter.WallRunStartRange))
        {
            return false;
        }

        if (hit.collider.gameObject.layer == _parameter._runWallLayer)
            {
                _nomal_vector = hit.normal;
                return true;
            }   
        
        return false;
    }
    private bool IsUpWallHit()
    {
        Ray up_ray = new Ray(transform.position, transform.up);
        return Physics.Raycast(up_ray, _parameter._ceilingRange, _parameter._groundLayer);
    }
    private bool IsGround()
    {
        Ray down_ray = new Ray(transform.position + (transform.up / 2.0f), -transform.up);
        return Physics.Raycast(down_ray, _parameter._groundRange, _parameter._groundLayer);
    }

    //-------------------------------------------------
    // アニメーションイベントで呼び出す奴
    //-------------------------------------------------
    private void AtackEvent()
    {
        AttackCollider.SetActive(true);
    }
    private void AtackEndEvent()
    {
        AttackCollider.SetActive(false);

    }

    //-------------------------------------------------
    // スライディング中に起動するやつ
    //-------------------------------------------------
    private void SetSlidingCollider(bool isSliding)
    {
        if (isSliding)
        {
            _box_collider.size = new Vector3(_collider_size.x, _collider_size.y / 2.0f, _collider_size.z);
            _box_collider.center = new Vector3(_collider_center.x, _collider_center.y / 2.0f, _collider_center.z);
            return;
        }
        _box_collider.size = _collider_size;
        _box_collider.center = _collider_center;
    }

    //-------------------------------------------------
    // 各種取得関数
    //-------------------------------------------------    
    public PlayerStateID GetState()
    {
        return StateProcessor.State.GetState();
    }
}
