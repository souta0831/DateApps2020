using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using State;
using DG.Tweening;
using UnityEngine.Playables;

public class BossEnemy : MonoBehaviour
{
    private GameObject _endCollision;
    //
    [SerializeField]
    private Camera _movieCamera;
    [SerializeField]
    private BossParameter _parameter;
    //ステート
    private StateProcessor _stateProcessor = new StateProcessor();
    private BossStateFall StateFall = new BossStateFall();
    private BossStateChase StateChase = new BossStateChase();
    private BossStateRise StateRise = new BossStateRise();
    //コンポーネント
    private PlayableDirector _playableDirector;
    private bool _isActive = false;
    
    public GameObject _endColliderArea { private get; set; }
    void Start()
    {
        //初期ステートセット
        _stateProcessor.State = StateFall;
        //イデレーターにセット
        StateFall.execDelegate = FallState;
        StateChase.execDelegate = ChaseState;
        StateRise.execDelegate = RiseState;
        _playableDirector = GetComponent<PlayableDirector>();


    }

    public void BossStart()
    {
        _isActive = true;
        GameManager.ChangeCamera(_movieCamera);
        _playableDirector.Play();
    }
    void Update()
    {
        if (!_isActive) return;
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
        //Vector3 fall = -transform.up * _parameter.FallSpeed;
        //transform.position += fall;
        //if (IsGround())
        //{
        //    _stateProcessor.State = StateChase;

        //}
    }
    //追いかける
    private void ChaseState()
    {
        Vector3 moveDis=(transform.position + _endColliderArea.transform.position).normalized;
        moveDis.y = 0;
        transform.position += moveDis * _parameter.ChaseSpeed;
        
    }
    //上昇
    private void RiseState()
    {
        Vector3 rise = transform.up * _parameter.RiseSpeed;

        transform.position += rise;
    }
    
    private bool IsGround()
    {
        Ray down_ray = new Ray(transform.position , -transform.up);
        return Physics.Raycast(down_ray, 1, _parameter.GroundLayer);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ボス:トリガーヒット");

        if (other.gameObject== _endColliderArea)
       {
            _stateProcessor.State = StateRise;

        }
    }
}
