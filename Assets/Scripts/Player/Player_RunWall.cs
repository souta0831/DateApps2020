using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RunWall : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_runWallLayer = 10;

    [SerializeField]
    private float m_wallCheckRange = 1.0f;

    public bool RunWallHitCheck(Vector3 _direction)
    {
        Ray ray = new Ray(transform.position, _direction);
        return Physics.Raycast(ray ,m_wallCheckRange, m_runWallLayer);
    }
}
