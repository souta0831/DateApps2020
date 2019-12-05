using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoter : MonoBehaviour
{
    enum Arm
    {
        Left,
        Right
    }
    [SerializeField]
    BulletDate LeftArm;
    [SerializeField]
    BulletDate RightArm;
    [SerializeField]
    bool _isActive;
    [SerializeField]
    private GameObject _targetObject = null;

    private Animator _animator = default;
    private float[] _fire_timer = new float[2];
    private float[] _cool_timer = new float[2];
    private float[] _burst_count = new float[2];

    public bool IsActive
    {
        set { _isActive = value; }
    }
    public GameObject TargetObject
    {
        set { _targetObject = value; }
    }
    private void Start()
    {
        //左腕
        _fire_timer[(int)Arm.Left] = LeftArm._parameter.FireRate;
        _cool_timer[(int)Arm.Left] = LeftArm._parameter.FireCoolTime;
        _burst_count[(int)Arm.Left] = LeftArm._parameter.Burst;
        //右腕
        _fire_timer[(int)Arm.Right] = RightArm._parameter.FireRate;
        _cool_timer[(int)Arm.Right] = RightArm._parameter.FireCoolTime;
        _burst_count[(int)Arm.Right] = RightArm._parameter.Burst;

        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_isActive) return;
        LeftShoter();
        RightShoter();
        
    }
    void LeftShoter()
    {
        _cool_timer[(int)Arm.Left]--;
        if (_cool_timer[(int)Arm.Left] >= 0) return;
        _fire_timer[(int)Arm.Left] --;
        if (_fire_timer[(int)Arm.Left] >= 0) return;

        _animator.SetTrigger("LeftShot");
        Debug.Log("左");
        //発射感覚をリセット
        _fire_timer[(int)Arm.Left] = LeftArm._parameter.FireRate;
        //球数を減らす
        _burst_count[(int)Arm.Left]--;
        //リロード
        if (_burst_count[(int)Arm.Left] <= 0)
        {
            _cool_timer[(int)Arm.Left] = LeftArm._parameter.FireCoolTime;
            _burst_count[(int)Arm.Left] = LeftArm._parameter.Burst;
        }


    }
    public void OnLeftShot()
    {
        //発射
        var bullet = Instantiate(LeftArm._bullet, LeftArm._shotPos.position, LeftArm._shotPos.rotation);
        bullet.transform.parent = this.transform.root.gameObject.transform;
        var bullet_scprit = bullet.GetComponent<BalletBase>();
        bullet_scprit.TargetObject = _targetObject;
        bullet_scprit.Accuracy = LeftArm._parameter.Accuracy;

    }
    void RightShoter()
    {
        _cool_timer[(int)Arm.Right] --;
        if (_cool_timer[(int)Arm.Right] >= 0) return;
        _fire_timer[(int)Arm.Right] --;
        if (_fire_timer[(int)Arm.Right] >= 0) return;

        _animator.SetTrigger("RightShot");
        Debug.Log("右");
        //発射感覚をリセット
        _fire_timer[(int)Arm.Right] = RightArm._parameter.FireRate;
        //球数を減らす
        _burst_count[(int)Arm.Right]--;
        //リロード
        if (_burst_count[(int)Arm.Right] <= 0)
        {
            _cool_timer[(int)Arm.Right] = RightArm._parameter.FireCoolTime;
            _burst_count[(int)Arm.Right] = RightArm._parameter.Burst;
        }

    }
    public void OnRightShot()
    {
        //発射
        var bullet = Instantiate(RightArm._bullet, RightArm._shotPos.position, RightArm._shotPos.rotation);
        bullet.transform.parent = this.transform.root.gameObject.transform;
        var bullet_scprit = bullet.GetComponent<BalletBase>();
        bullet_scprit.TargetObject = _targetObject;
        bullet_scprit.Accuracy = RightArm._parameter.Accuracy;

    }
}
