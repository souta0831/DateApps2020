using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Ballet : BalletBase
{
    [SerializeField]
    private GameObject _muzzleFlashPrefab;

    private SpeedManager _speedManager;

    protected override void Initialize()
    {
        //速度を取得する
        _speedManager = transform.root.gameObject.GetComponent<SpeedManager>();

        //ぶれを求める
        Vector3 accuracy = new Vector3(Random.Range(-_accuracy, _accuracy), 0, Random.Range(-_accuracy, _accuracy));
        //ターゲットのほうに弾を向ける
        transform.LookAt(_targetObject.transform.position);
        _rigidBody.velocity= transform.forward * (_speed * _speedManager.speedProperty.Value); ;
        _speedManager.speedProperty.DistinctUntilChanged().Subscribe(_count => { ChangeSpeedVec(); });
        Instantiate(_muzzleFlashPrefab, transform.position, transform.rotation);

    }
    protected override void Move()
    {
        transform.eulerAngles += new Vector3(0, 0, 2);    
    }
    private void ChangeSpeedVec()
    {
       // Debug.Log("値変わった？");
       // _rigidBody.velocity=transform.forward * (_speed * _speedManager.speedProperty.Value);

    }
    protected override void FixedMove()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
           // Destroy(this.gameObject);
    }
}
