using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBallet : BalletBase
{

    //速度
    Vector3 _velocity;
    //発射するときの初期位置
    Vector3 _targetPosition;
    // 加速度
     Vector3 _acceleration;
    // 着弾時間
    [SerializeField]
    float _homingTime = 10;
    [SerializeField]
    float _homingPower = 30;
    float _rotation = 0;



    protected override void Initialize()
    {
        _targetPosition = _targetObject.transform.position;
    }   
    protected override void Move()
    {
        _acceleration = Vector3.zero;

        //ターゲットと自分自身の差
        var diff = _targetObject.transform.position - transform.position;

        //加速度を求めてるらしい
        _acceleration += (diff - _velocity * _homingTime) * 2f/ (_homingTime * _homingTime);


        //加速度が一定以上だと追尾を弱くする
        if (_acceleration.magnitude > _homingPower)
        {
            _acceleration = _acceleration.normalized * _homingPower;
        }


        // 速度の計算

        if (_homingTime <= 0) return;
        // 着弾時間を徐々に減らしていく
        _velocity += _acceleration * Time.deltaTime;
        _homingTime -= Time.deltaTime;

    }
    protected override void FixedMove()
    {
        _rigidBody.MovePosition(transform.position + _velocity * Time.deltaTime);

    }
}
