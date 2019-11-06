using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using State;

public class BossEnemy : MonoBehaviour
{
    private GameObject _endCollision;
    //
    [SerializeField]
    private BossParameter Parameter;
    //ステート
    private StateProcessor _stateProcessor = new StateProcessor();
    private BossStateFall StateFall = new BossStateFall();
    private BossStateChase StateChase = new BossStateChase();
    private BossStateRise StateRise = new BossStateRise();
    void Start()
    {
        //初期ステートセット
        _stateProcessor.State = StateFall;
        //イデレーターにセット
        StateFall.execDelegate = FallState;
        StateChase.execDelegate = ChaseState;
        StateRise.execDelegate = RiseState;
    }

    void Update()
    {
        State();
    }
    //ステート関数
    private void State()
    {
        if (_stateProcessor.State == null)
        {
            return;
        }
        _stateProcessor.Execute();
    }
    //下降
    private void FallState()
    {
        Vector3 fall = -transform.up * Parameter.FallSpeed;
        transform.position -= fall;
    }
    //追いかける
    private void ChaseState()
    {

    }
    //上昇
    private void RiseState()
    {

    }
}
