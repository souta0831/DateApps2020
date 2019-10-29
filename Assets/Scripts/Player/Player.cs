using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerParameter Parameter;

    [SerializeField]
    private LifePointBase _lifePoint = null;

    [SerializeField]
    private Player_EffectManager _EffectManager = null;

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

    private bool _isLockOn = false;
    private int _lockon_num = 0;
    
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
        _lifePoint.MaxPointSet(Parameter.MaxHp);
        _lifePoint.PointSet(Parameter.MaxHp);
        //Effect停止
        _EffectManager.AllParticleStop();
    }

    void Update()
    {
        State();
        Jump();
        //LockOnUpdate();
        StickUpdate();
        Move();
    }
    //-------------------------------------------------
    // 移動の関数のやつ？
    //-------------------------------------------------
    private void Move()
    {
        transform.position += _move_power;
        _buffer_pos = transform.position;
    }

    private void Jump()
    {


        if (InputController.GetButtonDown(Button.B) && IsGround())
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddRelativeForce(transform.up * Parameter.JumpPower);
            StateProcessor.State = StateJump;
        }



    }

    private void StickUpdate()
    {
        _stick_x = InputController.GetAxisRow(Axis.L_Horizontal);
        _stick_z = InputController.GetAxisRow(Axis.L_Vertical);
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

        if (InputController.GetAxisDown(Axis.L_Horizontal) != 0 || InputController.GetAxisDown(Axis.L_Vertical) != 0)
        {
            StateProcessor.State = StateRun;
        }
        _move_power *= 0.9f;

    }
    private void RunState()
    {
        _animator.SetBool("is_running", true);


        Vector3 camForward = Vector3.Scale(Camera.transform.forward, Vector3.right + Vector3.forward);
        Vector3 moveForward = (camForward * _stick_z) + (Camera.transform.right * _stick_x);
        _move_power = moveForward * Parameter.RunSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveForward);


        if (InputController.GetButtonDown(Button.R1))
        {
            //スライディングに移行
            StateProcessor.State = StateSliding;
            SetSlidingCollider(true);
            _EffectManager.AllParticlePlay();


        }
        if (_stick_x == 0.0f && _stick_z == 0.0f)
        {
            Debug.Log("ステート移行");
            StateProcessor.State = StateIdle;
        }

    }
    private void SlidingState()
    {
        _animator.SetBool("is_sliding", true);

        var moveForward = Vector3.Scale(transform.forward, new Vector3(1.0f, 0.0f, 1.0f));
        var moveLR = (transform.right * _stick_x).normalized;
        _move_power = (moveForward * Parameter.SlidingSpeed + moveLR * Parameter.SlidingLRSpeed) * Time.deltaTime;

        if (InputController.GetButtonDown(Button.Y))
        {

            _animator.SetTrigger("Attack");

        }

        if (!InputController.GetButtonStay(Button.R1) && !IsUpWallHit() && IsGround())
        {
            //アイドルに移行
            StateProcessor.State = StateIdle;
            _EffectManager.AllParticleStop();
            SetSlidingCollider(false);
        }


    }
    private void JumpState()
    {
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
        _move_power = WallRunVector * Parameter.WallRunSpeed;
    }
    //-------------------------------------------------
    // 判定関数
    //-------------------------------------------------
    private bool WallRunStartCheck()
    {
        _move_direction = (transform.position - _buffer_pos).normalized;
        Ray ray = new Ray(transform.position, _move_direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Parameter.WallRunStartRange))
        {
            if (hit.collider.gameObject.layer == 10)
            {
                _nomal_vector = hit.normal;
                return true;
            }
        }
        return false;

    }
    private bool IsUpWallHit()
    {
        Ray up_ray = new Ray(transform.position, transform.up);

        if (Physics.Raycast(up_ray,Parameter._ceilingRange, Parameter._groundLayer))
        {
            return true;
        }
        return false;
    }
    private bool IsGround()
    {
        Ray down_ray = new Ray(transform.position + (transform.up / 2.0f), -transform.up);
        if (Physics.Raycast(down_ray, Parameter._groundRange, Parameter._groundLayer))
        {
            return true;

        }

        return false;
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
            _box_collider.size = new Vector3(_collider_size.x, _collider_size.y / 2, _collider_size.z);
            _box_collider.center = new Vector3(_collider_center.x, _collider_center.y / 2, _collider_center.z);
            return;
        }
        _box_collider.size = _collider_size;
        _box_collider.center = _collider_center;
    }
    //-------------------------------------------------
    // ロックオンの処理
    //-------------------------------------------------
    private void LockOnUpdate()
    {
        if (_isLockOn)
        {
            if (InputController.GetButtonDown(Button.L1) || Parameter.LockOnRange <=  _enemyManager.GetEnemyList()[_lockon_num].GetPlayerDistance())
            {
                Debug.Log("ロックオン解除");
              _lockonCursor.OnLockonEnd();
                _isLockOn = false;
            }

            return;
        }

        if (Parameter.LockOnRange >=  _enemyManager.GetEnemyList()[_lockon_num].GetPlayerDistance())
        {
          _lockonCursor.OnLockonRady( _enemyManager.GetEnemyList()[_lockon_num].gameObject);
        }
        //ロックオン　
        if (InputController.GetButtonDown(Button.L1))
        {
          _lockonCursor.OnLockonStart();
            _isLockOn = true;
        }
        //ロック切り替え
        if (InputController.GetButtonDown(Button.RightStick))
        {
            if (Parameter.LockOnRange >=  _enemyManager.GetEnemyList()[_lockon_num + 1].GetPlayerDistance())
            {
                _lockon_num++;
            }
        }

    }

    //-------------------------------------------------
    // 各種取得関数
    //-------------------------------------------------
    public GameObject GetLockOnbject()
    {
        return _lockonCursor.GetLockONTarget();
    }
    public bool IsLockOn()
    {
        return _isLockOn;
    }
    public PlayerStateID GetState()
    {
        return StateProcessor.State.GetState();
    }
}
