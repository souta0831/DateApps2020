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

        _animator.SetTrigger("LeftShot");
        _cool_timer[(int)Arm.Left] = LeftArm._parameter.FireCoolTime;


    }
    public void OnLeftShot()
    {

        StartCoroutine(Shot(LeftArm));
        Debug.Log("左");
    }
    void RightShoter()
    {
        _cool_timer[(int)Arm.Right] --;
        if (_cool_timer[(int)Arm.Right] >= 0) return;

        _animator.SetTrigger("RightShot");
        _cool_timer[(int)Arm.Right] = RightArm._parameter.FireCoolTime;
    }
    public void OnRightShot()
    {

        StartCoroutine(Shot(RightArm));
        Debug.Log("右");
    }

    private IEnumerator Shot(BulletDate date)
    {
        for(int i = 0; i < date._parameter.Burst ; i++)
        {
            var bullet = Instantiate(date._bullet, date._shotPos.position, date._shotPos.rotation);
            bullet.transform.parent = this.transform.root.gameObject.transform;
            var bullet_scprit = bullet.GetComponent<BalletBase>();
            bullet_scprit.TargetObject = _targetObject;
            bullet_scprit.Accuracy = date._parameter.Accuracy;
            Debug.Log("弾発射" + i);
            yield return new WaitForSeconds(date._parameter.FireRate/60);

        }
    }
}
