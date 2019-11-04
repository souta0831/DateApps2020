using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "custom/BossController", fileName = "BossParameter")]

public class BossParameter : ScriptableObject
{
    //落下速度
    [SerializeField]
    public float FallSpeed=1;
    //追いかける速度
    [SerializeField]
    public float ChaseSpeed=1;
    //上昇速度
    [SerializeField]
    public float RiseSpeed = 1;
}
