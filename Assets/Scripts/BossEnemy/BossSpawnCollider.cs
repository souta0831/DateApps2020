using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnCollider : MonoBehaviour
{

    public BossManager _bossManager {private get; set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _bossManager.SpawnBoss();
        }
    }
}
