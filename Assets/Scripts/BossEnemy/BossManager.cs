using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //このオブジェクトにプレイヤーが当たったら生成
    [SerializeField]
    private BossSpawnCollider _bossSpawnCollider;
    [SerializeField]
    private GameObject _bossEndCollision;
    [SerializeField]
    private GameObject _boss;
    //ボスのスクリプト
    private BossEnemy _boosScript;
    void Start()
    {
        _bossSpawnCollider._bossManager = this;
        _boosScript = _boss.GetComponent<BossEnemy>();
        _boosScript._endColliderArea = _bossEndCollision;

    }
    public void SpawnBoss()
    {
        Debug.Log("Boss:Spawn");
        //_boosScript.BossStart();
        Destroy(_bossSpawnCollider.gameObject); 
    }
}
