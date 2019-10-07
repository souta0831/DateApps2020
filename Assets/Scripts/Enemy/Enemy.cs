using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private Transform _player_transform=null;
    private bool _is_activate = false;
    //起動に必要な距離
    [SerializeField] private float _active_distance=50;
    //生成する弾
    [SerializeField] private GameObject _bullet;
    //発射レート
    [SerializeField] private float FireRate=5;
    //再発射までの時間
    [SerializeField] private float FireCoolTime=60;
    //連射数
    [SerializeField] private int Burst = 3;
    private float _fire_timer;
    private float _cool_timer;
    private float _burst_count ;
    private GameObject[] _bullet_list = new GameObject[3];
    void Start()
    {
        _fire_timer = FireRate;
        _burst_count  = Burst;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player_transform == null) return;
        ActiveCheck();
        if (!_is_activate) return;
        CreateBullet();
    }
    public void SetPlayer(Transform player)
    {
        _player_transform = player;
    }
    private void ActiveCheck()
    {
        if (_is_activate) return;
        float distance = Vector3.Distance(transform.position, _player_transform.position);
        Debug.Log(distance);
        if (_active_distance > distance)
        {
            _is_activate = true;
            Debug.Log("起動");
        }
    }
    private void CreateBullet()
    {
        _cool_timer--;
        if (_cool_timer >= 0) return;
        _fire_timer--;
        if (_fire_timer >= 0) return;

        var bullet = Instantiate(_bullet, transform.position,transform.rotation);
        Vector3 angle = ((_player_transform.position+new Vector3(0,0.5f,0)) - transform.position).normalized;
        bullet.GetComponent<Ballet>().SetAngle(angle);
        _fire_timer = FireRate;
        _burst_count--;
        if (_burst_count <= 0)
        {
            _cool_timer = FireCoolTime;
            _burst_count = Burst;
        }


    }
}
