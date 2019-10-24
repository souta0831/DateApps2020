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
    public float SlidingSpeed = 6.0f;
    [SerializeField]
    public float SlidingLRSpeed = 2.0f;
    [SerializeField]
    public float LockOnRange = 10.0f;
    [SerializeField]
    public int MaxHp = 100;
}
