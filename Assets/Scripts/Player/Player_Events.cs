using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Events : MonoBehaviour
{
    [SerializeField]
    private GameObject _attackColliderObject = default;

    private void Start()
    {
        _attackColliderObject.SetActive(false);
    }

    private void AttackEvent()
    {
        _attackColliderObject.SetActive(true);
        Debug.Log("Player::攻撃");
    }
    private void AttackExitEvent()
    {
        _attackColliderObject.SetActive(false);
        Debug.Log("Player::攻撃終了");
    }
}
