using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BulletDate
{
    public ShooterParameter _parameter;
    public GameObject _bullet;
    public Transform _shotPos;

}
public class EnemyShooter : MonoBehaviour
{
    //発射する玉のデータ
    [SerializeField]
    List<BulletDate> Bullets;
    //シュータが起動中かどうか
    [SerializeField]
    bool _isActive;



    private GameObject _targetObject = null;
    private Enemy _enemy;
    private List<float> _fire_timer=new List<float>();
    private List<float> _cool_timer = new List<float>();
    private List<float> _burst_count = new List<float>();

    //パラメーター
    public bool IsActive
    {
        set { _isActive = value; }
    }
    public GameObject TargetObject
    {
        set { _targetObject = value; }
    }
    void Start()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            _fire_timer.Add(Bullets[i]._parameter.FireRate);
            _cool_timer.Add(Bullets[i]._parameter.FireCoolTime);
            _burst_count.Add(Bullets[i]._parameter.Burst);
        }
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) return;
        CreateBullet();


    }
    //弾生成関数
    private void CreateBullet()
    {
        if (_targetObject == null)
        {
            Debug.Log("ターゲットがセットされてません");
            return;
        }
        for(int i = 0; i<Bullets.Count;i++)
        {
            _cool_timer[i]--;
            if (_cool_timer[i] >= 0) return;
            _fire_timer[i]--;
            if (_fire_timer[i] >= 0) return;

            var bullet = Instantiate(Bullets[i]._bullet, Bullets[i]._shotPos.position, Bullets[i]._shotPos.rotation);

            var bullet_scprit = bullet.GetComponent<BalletBase>();
            bullet_scprit.TargetObject = _targetObject;
            bullet_scprit.Accuracy = Bullets[i]._parameter.Accuracy;

            _fire_timer[i] = Bullets[i]._parameter.FireRate;
            _burst_count[i]--;
            if (_burst_count[i] <= 0)
            {
                _cool_timer[i] = Bullets[i]._parameter.FireCoolTime;
                _burst_count[i] = Bullets[i]._parameter.Burst;
            }

        }


    }

}
