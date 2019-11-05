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
    private GameObject _bossSpawnCollision;
    [SerializeField]
    private GameObject _bossEndCollision;
    [SerializeField]
    private GameObject _bossPrefab;
    //ボス本体
    private GameObject _boss;
    //ボスのスクリプト
    private BossEnemy _boosScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnBoss()
    {
        if (_boss != null) return;
        _boss = Instantiate(_bossPrefab, _spawnPos);
        _boosScript = GetComponent<BossEnemy>();
    }
}
