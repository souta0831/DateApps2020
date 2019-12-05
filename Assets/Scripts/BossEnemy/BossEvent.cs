using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    BossBeam _bossBeam;
    BossShoter _bossShoter;
    private void Start()
    {
        _bossBeam = GetComponent<BossBeam>();
        _bossShoter = GetComponent<BossShoter>();
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
    private void RightAttackEnd()
    {
        _bossShoter.OnRightShot();
    }
    private void LeftAttackEnd()
    {
        _bossShoter.OnLeftShot();

    }
}
