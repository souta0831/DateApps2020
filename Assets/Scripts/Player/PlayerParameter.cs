using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "custom/PlayerController", fileName = "PlayerParameter")]

public class PlayerParameter : ScriptableObject
{
    [SerializeField]
    public float RunSpeed = 3.0f;
    [SerializeField]
    public float JumpPower = 100.0f;
    [SerializeField]
    public float FallPower = 10.0f;
    [SerializeField]
    public float SlidingSpeed = 6.0f;
    [SerializeField]
    public float SlidingLRSpeed = 2.0f;
    [SerializeField]
    public float LockOnRange = 10.0f;
    [SerializeField]
    public int MaxHp = 100;
    [SerializeField]
    public int MaxBoostPoint = 10000;
    [SerializeField]
    public float WallRunStartRange = 1;
    [SerializeField]
    public float WallRunSpeed = 3.0f;
    [SerializeField]
    public LayerMask _groundLayer = 9;
    [SerializeField]
    public LayerMask _runWallLayer = 10;
    [SerializeField]
    public float _groundRange = 1;
    [SerializeField]
    public float _ceilingRange = 2;

}
