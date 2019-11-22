using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballet : BalletBase
{
    
    protected override void Initialize()
    {
        //ぶれを求める
        Vector3 accuracy = new Vector3(Random.Range(-_accuracy, _accuracy), 0, Random.Range(-_accuracy, _accuracy));
        //ターゲットのほうに弾を向ける
        transform.LookAt(_targetObject.transform.position);

    }
    protected override void Move()
    {
        _rigidBody.AddForce(transform.forward * _speed); 

    }
}
