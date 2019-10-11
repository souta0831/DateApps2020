using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    List<Enemy> EnemyList;
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Enemy>() != null) {
                EnemyList.Add(child.gameObject.GetComponent<Enemy>());
            }
        }
        foreach (Enemy enemy_list in EnemyList)
        {
            enemy_list.SetPlayer(_player.GetComponent<Transform>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
