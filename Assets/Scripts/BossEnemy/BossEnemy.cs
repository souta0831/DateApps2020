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
    //ダウンするまでに弾を充てる回数
    [SerializeField]
    int _downHp=1;
    int _nowHP=0;
    //
    [SerializeField]

    bool _isDown;
    Vector3 _startPos=Vector3.zero;
    [SerializeField]
    float _nowPosLeap =0;
    //ステート
    private StateProcessor _stateProcessor = new StateProcessor();
    private BossStateFall StateFall = new BossStateFall();
    private BossStateChase StateChase = new BossStateChase();
    private BossStateRise StateRise = new BossStateRise();
    //コンポーネント
    private PlayableDirector _playableDirector;
    private bool _isActive = false;
    private Animator _animator;
    //弾の発射機構
    private BossShoter _shoter;
    public GameObject _endColliderArea { private get; set; }
    void Start()
    {
        _shoter = GetComponent<BossShoter>();
        _animator =GetComponent<Animator>();
        _nowHP = _downHp;
        //初期ステートセット
        _stateProcessor.State = StateFall;
        _startPos = transform.position;
    }
    void Update()
    {

        PosLeap();
        OnDown();
    }
    void PosLeap()
    {
        var z = Mathf.Lerp(_startPos.z, 0, _nowPosLeap);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
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
    private void OnDown()
    {
        _animator.SetBool("IsDown", _isDown);
        _isDown = _nowHP <= 0;

        if (_isDown)
        {
            _shoter.IsActive = false;
            _nowPosLeap = Math.Min(0.8f, _nowPosLeap += 0.005f);
            return;
        }
        _nowPosLeap = Math.Max(_nowPosLeap -= 0.01f, 0);
        _shoter.IsActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RifrectBallet")
        {
            Debug.Log("ボスヒット");
            _nowHP--;
            _animator.SetTrigger("Hit");
        }
        if (other.gameObject.tag == "Attack")
        {
            _nowHP = _downHp;
            _animator.SetTrigger("DownEnd");

        }

    }
}
