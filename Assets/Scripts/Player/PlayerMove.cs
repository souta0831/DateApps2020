using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState;



public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    //変更前のステート名
    private string _bufferStateName;
    //プレイヤーの情報
    Transform _transfrom;
    CharacterController _character;
    //ステート
    public StateProcessor     StateProcessor=new StateProcessor();
    public PlayerStateIdle    StateIdle     =new PlayerStateIdle();
    public PlayerStateRun     StateRun      =new PlayerStateRun();
    public PlayerStateSliding StateSliding  =new PlayerStateSliding();
    //制御変数
    [SerializeField] float RunSpeed=2.0f;
    [SerializeField] float SlidingSpeed = 4.0f;
    void Start()
    {
        StateProcessor.State = StateIdle;
        StateIdle.execDelegate = Idle;
        StateRun.execDelegate = Run;
        StateSliding.execDelegate = Sliding;
        _transfrom = this.GetComponent<Transform>();
        _character = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
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
        float i = InputController.GetAxis(AxisID.L_Horizontal);
        if (InputController.GetAxis(AxisID.L_Horizontal) != 0 || InputController.GetAxis(AxisID.L_Vertical) != 0)
        {
            StateProcessor.State = StateRun;
        }
    } 
    private void Run()
    {


        float v = InputController.GetAxis(AxisID.L_Vertical); ;
        float h = InputController.GetAxis(AxisID.L_Horizontal);
        Vector3 lookPos = transform.position + new Vector3(h, 0, v);
        transform.LookAt(lookPos);
        var velocity = new Vector3(h, 0, v).normalized;
        velocity *= RunSpeed;

        _character.Move(velocity * Time.deltaTime);

        if (InputController.GetAxis(AxisID.L_Horizontal) == 0 && InputController.GetAxis(AxisID.L_Vertical) == 0)
            StateProcessor.State = StateIdle;
    }
    private void Sliding()
    {

    }
}
