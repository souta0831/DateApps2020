using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Events : MonoBehaviour
{
    [SerializeField]
    private GameObject m_attackColliderObject = default;

    [SerializeField]
    private GameObject m_trailRendererObject = default;

    private void Start()
    {
        m_attackColliderObject.SetActive(false);
    }

    private void AttackEvent()
    {
        m_attackColliderObject.SetActive(true);
        Debug.Log("Player::攻撃");
    }
    private void AttackExitEvent()
    {
        m_attackColliderObject.SetActive(false);
        Debug.Log("Player::攻撃終了");
    }

    private void TrailEvent()
    {
        m_trailRendererObject.SetActive(true);
    }
    private void TrailExitEvent()
    {
        m_trailRendererObject.SetActive(false);
    }
}
