﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerParameter _parameter;

    private SpeedManager m_speedManager = default;

    [SerializeField]
    private EnemyManager  _enemyManager;

    [SerializeField]
    private LifePointBase _boostPoint = default;

    private LifePointBase _lifePoint = default;

    private Player_EffectManager _effectManager = default;

    //Thisコンポーネント
    private Rigidbody _rigidbody;
    private Animator _animator;
    private BoxCollider _box_collider;

    //カメラ
    private Camera Camera;

    //保存用
    private Vector3 _collider_size = Vector3.zero;
    private Vector3 _collider_center = Vector3.zero;
    private float _move_power = 0.0f;
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
    private PlayerStateRushAttack StateRushAttack = new PlayerStateRushAttack();
    private PlayerStateDamage StateDamage = new PlayerStateDamage();

    void Start()
    {
        //GetComponent
        Camera = Camera.main;

        _rigidbody = GetComponent<Rigidbody>();
       _animator = GetComponentInChildren<Animator>();
       _box_collider = GetComponent<BoxCollider>();
        _lifePoint = GetComponent<LifePointBase>();

        _effectManager = GetComponent<Player_EffectManager>();
        _effectManager.AllParticleStop();

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
        StateRushAttack.execDelegate = RushAttackState;
        StateDamage.execDelegate = DamageState;

        _lifePoint.MaxPointSet(_parameter.MaxHp);
        _lifePoint.PointSet(_parameter.MaxHp);
        _boostPoint.MaxPointSet(_parameter.MaxBoostPoint);
        _boostPoint.PointSet(_parameter.MaxBoostPoint);

        m_speedManager= transform.root.gameObject.GetComponent<SpeedManager>();
    }

    void Update()
    {
        State();
        StickUpdate();
        Jump();
        Move();

        if (InputController.GetButtonDown(Button.A))
        {
            Time.timeScale = 0.5f;
        }
        if (InputController.GetButtonUp(Button.A))
        {
            Time.timeScale = 1.0f;
        }
        
    }
    //-------------------------------------------------
    // 移動の関数のやつ？
    //-------------------------------------------------
    private void Move()
    {
        _rigidbody.velocity += (-transform.up * (_parameter.FallPower * Time.timeScale));
        //_rigidbody.transform.position += _move_power;
        m_speedManager.SetSpeed(_move_power);
        _buffer_pos = transform.position;
    }

    private void Jump()
    {
        if (InputController.GetButtonDown(Button.B) && IsGround())
        {
            _animator.SetTrigger("Jump");
            _rigidbody.velocity = (transform.up *  _parameter.JumpPower );
            StateProcessor.State = StateJump;
        }
    }

    private void StickUpdate()
    {
        _stick_x = Input.GetAxis("Horizontal");
        _stick_z = 1.0f;//Input.GetAxis("Vertical");
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
        _boostPoint.AddPoint(1);


        _animator.SetBool("is_running", false);
        _animator.SetBool("is_sliding", false);

        if (_stick_x != 0.0f || _stick_z != 0.0f)
        {
            StateProcessor.State = StateRun;
        }
        _move_power *= 0.9f;

    }
    private void RunState()
    {
        _boostPoint.AddPoint(1);

        if (_stick_x == 0.0f && _stick_z == 0.0f)
        {
            StateProcessor.State = StateIdle;
            return;
        }

        Vector2 _stickRange = new Vector2(_stick_x,_stick_z);
        _animator.SetBool("is_running", true);

        Vector3 camForward = Vector3.Scale(Camera.transform.forward, Vector3.right + Vector3.forward);
        Vector3 moveForward = transform.forward;
        this.transform.rotation = Quaternion.LookRotation(moveForward);

        this.transform.position += (transform.right * _stick_x) * 0.05f;

        _move_power = _parameter.RunSpeed * Time.deltaTime;

        if (InputController.GetButtonDown(Button.Y))
        {
            _animator.SetTrigger("Attack");
        }

        if (InputController.GetButtonDown(Button.X) )
        {
            _animator.SetBool("RushAttack",true);
            StateProcessor.State = StateRushAttack;
        }

        if (InputController.GetButtonDown(Button.R1))
        {
            //スライディングに移行
            StateProcessor.State = StateSliding;

            SetSlidingCollider(true);

            _effectManager.ParticlePlay(Player_EffectManager.EffectType.BurnerFire);
            _effectManager.ParticlePlay(Player_EffectManager.EffectType.SparkFire);
            _effectManager.ParticlePlay(Player_EffectManager.EffectType.SpeedLine);
        }

    }
    private void SlidingState()
    {
        _animator.SetBool("is_sliding", true);

        _boostPoint.AddPoint(-2);

        var moveForward = Vector3.Scale(transform.forward, Vector3.right + Vector3.forward);
        //var moveLR = (transform.right * _stick_x);

        //_move_power += (moveLR * _parameter.SlidingLRSpeed) /10.0f * Time.deltaTime;
        //if (_move_power.magnitude>= _parameter.SlidingLRSpeed)
        //{
        //   // _move_power = _move_power.normalized * _parameter.SlidingLRSpeed;
        //}
        _move_power =  _parameter.SlidingSpeed * Time.deltaTime;
        //_move_power.x *= 0.99f;

        if (InputController.GetButtonDown(Button.Y))
        {         
            _animator.SetTrigger("Attack");
        }

        if (!(InputController.GetButtonStay(Button.R1)) || _boostPoint.GetNowPoint() <= 0)
        {
            if (!ShouldSlidingExit()) {
                return;
            }
            //アイドルに移行
            StateProcessor.State = StateIdle;
            _effectManager.AllParticleStop();
            SetSlidingCollider(false);
        }
    }
    private void JumpState()
    {
        _effectManager.AllParticleStop();

        //if (WallRunStartCheck())
        //{
        //    Debug.Log("壁に当たった");
        //    StateProcessor.State = StateWalRun;
        //}
        if (IsGround())
        {
            StateProcessor.State = StateIdle;
        }
    }
    private void WallRunState()
    {
        Vector3 WallRunVector = _move_direction + Vector3.Scale(((-_move_direction + _nomal_vector).normalized), _nomal_vector);
        _move_power = _parameter.WallRunSpeed*Time.deltaTime;
    }

    private void RushAttackState()
    {
        Vector2 _stickRange = new Vector2(_stick_x, _stick_z);
        _animator.SetBool("is_running", true);

        Vector3 camForward = Vector3.Scale(Camera.transform.forward, Vector3.right + Vector3.forward);
        Vector3 moveForward = transform.forward;
        this.transform.rotation = Quaternion.LookRotation(moveForward);

        this.transform.position += (transform.right * _stick_x) * 0.05f;

        _move_power = _parameter.RunSpeed * Time.deltaTime;

        if (!InputController.GetButtonStay(Button.X) && IsGround())
        {
            _animator.SetBool("RushAttack", false);
            StateProcessor.State = StateRun;
        }
    }

    private void DamageState()
    {
        
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
        if (!IsFall()) return false;
        Ray down_ray = new Ray(transform.position + (transform.up / 2.0f), - transform.up);
        return Physics.Raycast(down_ray, _parameter._groundRange, _parameter._groundLayer);
    }
    private bool IsFall()
    {
        return _rigidbody.velocity.y <= 0.0f;
    }
    private bool ShouldSlidingExit()
    {
        return !(IsUpWallHit() || !IsGround());
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
    public StateBase GetState()
    {
        return StateProcessor.GetState();
    }
}
