using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;

public class Player : MonoBehaviour
{
   [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private float _jumpPower = 6.0f;
    [SerializeField]
    private float _sliding_speed = 6.0f;
    [SerializeField]
    private float _sliding_LR_speed = 2.0f;
    [SerializeField]
    private GameObject Slash_Efect=null;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private List<ParticleSystem> SlidingParticleList=new List<ParticleSystem>();

    private Rigidbody _rigidbody;
    private Animator _animator;
    //保存用
    private GameObject _temp_slash_fx=null;
    //ステート
    private StateProcessor StateProcessor = new StateProcessor();
    private PlayerStateIdle StateIdle = new PlayerStateIdle();
    private PlayerStateRun StateRun = new PlayerStateRun();
    private PlayerStateSliding StateSliding = new PlayerStateSliding();
    private PlayerStateAtack StateAtack = new PlayerStateAtack();

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        StateProcessor.State = StateIdle;
        StateIdle.execDelegate = Idle;
        StateRun.execDelegate = Run;
        StateSliding.execDelegate = Sliding;
        StateAtack.execDelegate = Atack;

        SlidingParticleSwitch(false);


    }

    void Update()
    {
        State();
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

        if (InputController.GetAxis(AxisID.L_Horizontal) != 0 || InputController.GetAxis(AxisID.L_Vertical) != 0)
        {
            StateProcessor.State = StateRun;
        }


    }
    private void Run()
    {
        _animator.SetBool("is_running", true);
        var x = InputController.GetAxis(AxisID.L_Horizontal);
        var z = InputController.GetAxis(AxisID.L_Vertical);

        Vector3 camForward = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = (camForward * z) + (Camera.transform.right * x);

        if (moveForward.magnitude > 0.01f)
        {
            transform.position += moveForward * _speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        if (InputController.GetButtonDown(ButtonID.R1)) 
        {
            StateProcessor.State = StateSliding;
            SlidingParticleSwitch(true);


        }
        if (InputController.GetAxis(AxisID.L_Horizontal) == 0 && InputController.GetAxis(AxisID.L_Vertical) == 0)
        {
            StateProcessor.State = StateIdle;

        }
    }
    private void Sliding()
    {
        _animator.SetBool("is_sliding", true);
   
        var x = InputController.GetAxis(AxisID.L_Horizontal);

        var moveForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        var moveLR = (transform.right* x).normalized;
        transform.position += (moveForward * _sliding_speed+ moveLR *_sliding_LR_speed)*Time.deltaTime;

        if (InputController.GetButtonDown(ButtonID.Y))
        {

            _animator.SetTrigger("Atack");

        }

        if (!InputController.GetButton(ButtonID.R1))
        {
            StateProcessor.State = StateIdle;
            SlidingParticleSwitch(false);

        }


    }
    private void Atack()
    {
        var moveForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        transform.position += (moveForward * _sliding_speed) * Time.deltaTime;

    }
    public PlayerStateID GetState()
    {
        return StateProcessor.State.GetState();
    }

    //アニメーションイベントで呼び出します
    private void AtackEvent()
    {
        StateProcessor.State = StateAtack;
        _temp_slash_fx = Instantiate(Slash_Efect, transform.position+new Vector3(0,0.5f,0),transform.rotation);
        _temp_slash_fx.GetComponent<ParticleSystem>().Play();
    }
    private void AtackEndEvent()
    {
        StateProcessor.State = StateIdle;
        Destroy(_temp_slash_fx);
        SlidingParticleSwitch(false);
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

}
