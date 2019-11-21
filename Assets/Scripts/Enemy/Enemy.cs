using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Transform _player_transform = null;
    private bool _is_activate = false;
    //起動に必要な距離
    [SerializeField]
    private float _active_distance = 50;
    //切られた時のエフェクト
    [SerializeField]
    private GameObject DeadParticle = null;
    [SerializeField]
    private List<GameObject> _deadObjectList;
    [SerializeField]
    private Transform _deadSpawnPos;
    private EnemyShooter _shooter;
    void Start()
    {
        _shooter = GetComponent<EnemyShooter>();
    }

    void Update()
    {
        if (_player_transform == null) return;
        ActiveCheck();
    }
    public void SetPlayer(Transform player)
    {
        _player_transform = player;
    }
    private void ActiveCheck()
    {
        if (_is_activate) return;
        //Rayを生成
        Ray ray = new Ray(transform.position, (_player_transform.position - transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _active_distance))
        {
            Debug.DrawLine(ray.origin, ray.direction, Color.red, 0, true);
            if (hit.collider.tag == "Player")
            {
                _is_activate = true;
                //シュータを起動させる
                _shooter.IsActive = true;
                //ターゲットをセットする
                _shooter.TargetObject = _player_transform.gameObject;
                Debug.Log("Enemy:起動");

            }

        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Debug.Log("攻撃");
            foreach (var deadObject in _deadObjectList)
            {
                Instantiate(DeadParticle, _deadSpawnPos.position, transform.rotation);

            }
            Destroy(this.gameObject);
        }
    }

    public float GetPlayerDistance()
    {

        return Vector3.Distance(this.transform.position, _player_transform.position);
    }
    public GameObject GetObject()
    {
        return gameObject;
    }

    public bool GetIsActive()
    {
        return _is_activate;
    }
    public Transform GetPlayerTrans()
    {
        return _player_transform;
    }

}
