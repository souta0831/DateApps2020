using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballet : BalletBase
{
    Vector3 _angle;

    protected override void Initialize()
    {
        Vector3 accuracy = new Vector3(Random.Range(-_accuracy, _accuracy), 0, Random.Range(-_accuracy, _accuracy));
        _angle = ((_targetObject.transform.position + new Vector3(0, 0.2f, 0)) - (transform.position + accuracy)).normalized;

    }
    protected override void Move()
    {
        transform.position += _angle * Speed * Time.deltaTime;

    }
    public void SetAngle(Vector3 angle) {
        _angle = angle;
    }
}
