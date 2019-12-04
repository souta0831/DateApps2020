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
    int _nowDonwHp = 0;
    [SerializeField]
    int _hp;
    int _nowHp;
    //
    [SerializeField]
    bool _isDown;
    Vector3 _startPos=Vector3.zero;
    [SerializeField]
    float _nowPosLeap =0;
    [SerializeField]
    private ParticleSystem HitEfect=default;
    //ステート
    private StateProcessor _stateProcessor = new StateProcessor();
    private BossStateIdle StateIdle = new BossStateIdle();
    private BossStateDown StateDown = new BossStateDown();
    private BossStateReturn StateReturn = new BossStateReturn();
    //コンポーネント
    private PlayableDirector _playableDirector;
    private Animator _animator;
    private BossBeam _bossBeam;
    //弾の発射機構
    private BossShoter _shoter;
    //保存用
    private bool _useLaser = false;
    public GameObject _endColliderArea { private get; set; }
    void Start()
    {
        _shoter = GetComponent<BossShoter>();
        _animator =GetComponent<Animator>();
        _bossBeam = GetComponent<BossBeam>();
        _nowDonwHp = _downHp;
        _nowHp = _hp;
        //初期ステートセット
        _stateProcessor.State = StateIdle;
        //イデレーター
        StateIdle.execDelegate = IdleState;
        StateDown.execDelegate = DownState;
        _startPos = transform.position;
    }
    void Update()
    {
        PosLeap();
        State();
        if (_nowHp == 1&& !_useLaser)
        {
            _bossBeam.OnVerticalBeam();
            _useLaser = true;

        }
    }
    void IdleState()
    {
        _nowPosLeap = Math.Max(_nowPosLeap -= 0.01f, 0);
        _shoter.IsActive = true;
        _animator.SetBool("IsDown", false);
    }
    void DownState()
    {
        _nowPosLeap = Math.Min(0.8f, _nowPosLeap += 0.005f);
        _shoter.IsActive = false;
        if (_nowPosLeap >= 1.3f)
        {
            _stateProcessor.State = StateIdle;
        }
        _animator.SetBool("IsDown", true);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ReflectBullet")
        {
            _nowDonwHp--;
            _animator.SetTrigger("Hit");
            HitEfect.Play();
            if (_nowDonwHp <= 0)
            {
                _stateProcessor.State = StateDown;
            }
        }
        if (other.gameObject.tag == "Attack")
        {
            Debug.Log("ボスヒット");
            _nowDonwHp = _downHp;
            _nowHp--;
            _stateProcessor.State = StateIdle;
            _animator.SetTrigger("DownEnd");

        }

    }
}
