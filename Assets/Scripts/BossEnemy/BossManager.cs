using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //出現する座標
    [SerializeField]
    private Transform _spawnPos = null;
    //このオブジェクトにプレイヤーが当たったら生成
    [SerializeField]
    private BossSpawnCollider _bossSpawnCollider;
    [SerializeField]
    private GameObject _bossEndCollision;
    [SerializeField]
    private GameObject _bossPrefab;
    //ボス本体
    private GameObject _boss=null;
    //ボスのスクリプト
    private BossEnemy _boosScript;
    void Start()
    {
        _bossSpawnCollider._bossManager = this;

    }

    // Update is called once per frame
    void Update()
    {   

    }
    public void SpawnBoss()
    {
        if (_boss != null) return;
        Debug.Log("Boss:Spawn");
        _boss = Instantiate(_bossPrefab, _spawnPos.position,_spawnPos.rotation);
        _boosScript = _boss.GetComponent<BossEnemy>();
        _boosScript._endColliderArea = _bossEndCollision;
        Destroy(_bossSpawnCollider.gameObject); 
    }
}
