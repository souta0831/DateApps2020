using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    private ShooterParameter _parameter;
    //生成する弾
    [SerializeField] private GameObject _bullet;
    //生成する場所
    [SerializeField] private Transform _shot_pos;

    private Enemy _enemy;
    private float _fire_timer;
    private float _cool_timer;
    private float _burst_count;

    void Start()
    {
        _fire_timer = _parameter.FireRate;
        _burst_count = _parameter.Burst;
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemy.GetIsActive()) return;
        CreateBullet();


    }
    private void CreateBullet()
    {
        _cool_timer--;
        if (_cool_timer >= 0) return;
        _fire_timer--;
        if (_fire_timer >= 0) return;

        var bullet = Instantiate(_bullet, _shot_pos.position, _shot_pos.rotation);
        Vector3 accuracy = new Vector3(Random.Range(-_parameter.Accuracy, _parameter.Accuracy), 0, Random.Range(-_parameter.Accuracy, _parameter.Accuracy));
        Vector3 angle = ((_enemy.GetPlayerTrans().position + new Vector3(0, 0.2f, 0)) - (transform.position + accuracy)).normalized;

        bullet.GetComponent<Ballet>().SetAngle(angle);
        _fire_timer = _parameter.FireRate;
        _burst_count--;
        if (_burst_count <= 0)
        {
            _cool_timer = _parameter.FireCoolTime;
            _burst_count = _parameter.Burst;
        }


    }

}
