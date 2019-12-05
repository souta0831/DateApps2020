using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    BossBeam _bossBeam;
    private void Start()
    {
        _bossBeam = GetComponent<BossBeam>();
    }
    private void BeginChargeEfect()
    {
        Debug.Log("charge開始");
        _bossBeam.OnCharge();
    }
    private void BeginBeam()
    {
        Debug.Log("ビーム開始");
        _bossBeam.OnVerticalBeam();
    }
}
