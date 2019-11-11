using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBallet : BalletBase
{

    //速度
    Vector3 _velocity;
    //発射するときの初期位置
    Vector3 position;
    // 加速度
     Vector3 _acceleration;
    // 着弾時間
    [SerializeField]
    float _homingTime = 2f;

    // 正面とする軸を指定する
    public string Axis = "z";
    // 正面とする軸
    Vector3 frontAxis;

    protected override void Initialize()
    {
        position = transform.position;
    }
    protected override void Move()
    {
        _acceleration = Vector3.zero;

        //ターゲットと自分自身の差
        var diff = _targetObject.transform.position - transform.position;
        //加速度を求める
        _acceleration += (diff - _velocity * _homingTime) * 2f/ (_homingTime * _homingTime);
        //加速度が一定以上だと追尾を弱くする
        if (_acceleration.magnitude > 100f)
        {
            _acceleration = _acceleration.normalized * 100f;
        }
        //着弾時間を徐々に減らしていく
        _homingTime -= Time.deltaTime;
        //移動処理
        _velocity += _acceleration * Time.deltaTime;
    }
    protected override void FixedMove()
    {
        // 移動処理
        _rigidBody.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

}
