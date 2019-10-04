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
    private Camera _camera;

    private Rigidbody _rigidbody;
    private Animator _animator;
    //ステート
    private StateProcessor StateProcessor = new StateProcessor();
    private PlayerStateIdle StateIdle = new PlayerStateIdle();
    private PlayerStateRun StateRun = new PlayerStateRun();
    private PlayerStateSliding StateSliding = new PlayerStateSliding();
    //アニメーター
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        StateProcessor.State = StateIdle;
        StateIdle.execDelegate = Idle;
        StateRun.execDelegate = Run;
        StateSliding.execDelegate = Sliding;

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
        Debug.Log(StateProcessor.State.GetStateName());

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

        Vector3 camForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = (camForward * z) + (_camera.transform.right * x);

        if (moveForward.magnitude > 0.01f)
        {
            transform.position += moveForward * _speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        if (InputController.GetButtonDown(ButtonID.A)) 
        {
            StateProcessor.State = StateSliding;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles-new Vector3(60,0,0));

        }
        if (InputController.GetAxis(AxisID.L_Horizontal) == 0 && InputController.GetAxis(AxisID.L_Vertical) == 0)
        {
            StateProcessor.State = StateIdle;

        }

    }
    private void Sliding()
    {

        _animator.SetBool("is_sliding", true);
    
        var x = InputController.GetAxis(AxisID.L_Horizontal)/20;

        var moveForward = Vector3.Scale(transform.forward+new Vector3(x,0,0), new Vector3(1, 0, 1)).normalized;
        transform.position += moveForward * (_speed * 2) * Time.deltaTime;

        if (!InputController.GetButton(ButtonID.A))
        {
            StateProcessor.State = StateIdle;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(60, 0, 0));

        }


    }

}
