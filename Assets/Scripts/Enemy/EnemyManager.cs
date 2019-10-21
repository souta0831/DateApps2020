using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    List<Enemy> _enemy_list;
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Enemy>() != null) {
                _enemy_list.Add(child.gameObject.GetComponent<Enemy>());
            }
        }
        foreach (Enemy enemy_list in _enemy_list)
        {
            enemy_list.SetPlayer(_player.GetComponent<Transform>());
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RemoveList();
        DistanceSort();


    }
    void RemoveList()
    {
        //敵がNULLだったら除外する
        foreach (Enemy enemy_list in _enemy_list.ToArray())
        {
            if (enemy_list == null)
                _enemy_list.Remove(enemy_list);


        }

    }

    //プレイヤーと近い順にソートする
    void DistanceSort()
    {
        for (int i = 0; i < _enemy_list.Count - 1; i++)
        {
            for (int j = (_enemy_list.Count - 1); j > i; j--)
            {
                if (_enemy_list[j - 1].GetPlayerDistance() >= _enemy_list[j].GetPlayerDistance())
                {
                    Enemy temp = _enemy_list[j - 1];
                    _enemy_list[j - 1] = _enemy_list[j];
                    _enemy_list[j] = temp;

                }
            }
        }

    }
    public List<Enemy> GetEnemyList(){
        return _enemy_list;
    }
}
