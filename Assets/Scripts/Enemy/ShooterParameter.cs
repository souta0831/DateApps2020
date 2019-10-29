using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "custom/ShooterController", fileName = "ShooterParameter")]

public class ShooterParameter : ScriptableObject
{
     //発射レート
    [SerializeField]
    public float FireRate = 5;
    //再発射までの時間
    [SerializeField]
    public float FireCoolTime = 60;
    //連射数
    [SerializeField]
    public int Burst = 3;
    //弾の精度
    [SerializeField]
    public float Accuracy = 5;

};
