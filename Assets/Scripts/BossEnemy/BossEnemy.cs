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
    private ParticleSystem HitEfect=default;
    [SerializeField]
    private GameObject _deadEfectPrefab;
    [SerializeField]
    private GameObject _expEffect;
    [SerializeField]
    float _dyingEfectFrame = 120;
    [SerializeField]
    int _maxHp;
    [SerializeField]
    int _maxDownHp = 3;
    int _nowDownHp = 0;
    int _nowHp;
    float _dyingEfectCount;
    bool _isDown;
    Vector3 _startPos = Vector3.zero;
    float _nowPosLeap = 0;

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
    private bool _isEnd = false;
    public GameObject _endColliderArea { private get; set; }
    void Start()
    {
        _shoter = GetComponent<BossShoter>();
        _animator =GetComponent<Animator>();
        _bossBeam = GetComponent<BossBeam>();
        _nowDownHp = _maxDownHp;
        _nowHp = _maxHp;
        _dyingEfectCount = _dyingEfectFrame;
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
        if (IsDying())
        {
            _dyingEfectCount--;
            if (_dyingEfectCount<=0)
            {
                HitEfect.Play();
                _dyingEfectCount = _dyingEfectFrame;
            }
            if (_nowDownHp == 1)
            {
                if (!_useLaser)
                {
                    _useLaser = true;
                    _animator.SetTrigger("Leser");
                }
                if (!_bossBeam._beamEnd)
                {
                    _shoter.IsActive = false;
                }
                else
                {
                    _shoter.IsActive = true;
                }
            }
        }
    }
    void IdleState()
    {
        _nowPosLeap = Math.Max(_nowPosLeap -= 0.01f, 0);
        _shoter.IsActive = true;
        _animator.SetBool("IsDown", false);
        if (_bossBeam.IsNowBeam())
        {
            _shoter.IsActive = false;
        }
        else
        {
            _shoter.IsActive = true;
        }

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
            _nowDownHp--;
            _animator.SetTrigger("Hit");
            HitEfect.Play();
            Instantiate(_expEffect, other.gameObject.transform.position,other.gameObject.transform.rotation);
            if (_nowDownHp <= 0)
            {
                _stateProcessor.State = StateDown;
            }
        }
        if (other.gameObject.tag == "Attack")
        {
            Debug.Log("ボスヒット");
            _nowDownHp = _maxDownHp;
            if (IsDying())
            {
                OnDead();
                return;
            }
            _animator.SetTrigger("DownEnd");
            _nowHp--;
            _stateProcessor.State = StateIdle;

        }

    }
    bool IsDying()
    {
        return _nowHp <= 1; 
    }
    void OnDead()
    {
        Instantiate(_deadEfectPrefab, transform.position, transform.rotation);
        _animator.SetTrigger("Dead");
        _shoter.IsActive = false;
    }
}
