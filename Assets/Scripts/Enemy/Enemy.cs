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
    //弾の精度
    [SerializeField]
    private float Accuracy = 5;

    //切られた時のエフェクト
    [SerializeField] private GameObject DeadParticle = null;
    private float _fire_timer;
    private float _cool_timer;
    private float _burst_count ;
    void Start()
    {
        _fire_timer = FireRate;
        _burst_count  = Burst;
    }

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
        //Rayを生成
        Ray ray = new Ray(transform.position, (_player_transform.position  - transform.position).normalized);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.direction, Color.red,0,true);

        if (Physics.Raycast(ray, out hit, _active_distance))
        {
            if (hit.collider.tag == "Player")
            {
                _is_activate = true;
                Debug.Log("起動");

            }

        }

    }
    private void CreateBullet()
    {
        _cool_timer--;
        if (_cool_timer >= 0) return;
        _fire_timer--;
        if (_fire_timer >= 0) return;

        var bullet = Instantiate(_bullet, transform.position+new Vector3(0,1.5f,0),transform.rotation);
        Vector3 accuracy = new Vector3(Random.Range(-Accuracy, Accuracy), 0, Random.Range(-Accuracy, Accuracy));
        Vector3 angle = ((_player_transform.position + new Vector3(0, 0.2f, 0)) - (transform.position+accuracy)).normalized;

        bullet.GetComponent<Ballet>().SetAngle(angle);
        _fire_timer = FireRate;
        _burst_count--;
        if (_burst_count <= 0)
        {
            _cool_timer = FireCoolTime;
            _burst_count = Burst;
        }


    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Debug.Log("攻撃");
            Instantiate(DeadParticle, transform.position+new Vector3(0,1,0),transform.rotation);
            Destroy(this.gameObject);
        }
    }
    public GameObject GetTransForm()
    {
        return gameObject;
    }
}
