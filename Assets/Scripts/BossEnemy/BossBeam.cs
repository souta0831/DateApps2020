﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    [SerializeField]
    private GameObject _beamPrefab;
    [SerializeField]
    private GameObject _chargeEfectPrefab;
    [SerializeField]
    private Transform _beamPos;
    private ParticleSystem _particleSystem;
    private GameObject _beamObject;
    private Animator _animator;
    public bool _beamEnd = false;
    void Start()
    {
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        VerticalBeam();
    }
    public void OnVerticalBeam()
    {
        if (_beamObject == null)
        {
            _beamObject = Instantiate(_beamPrefab, _beamPos.position, _beamPos.rotation);
        }
    }
    public void OnCharge()
    {
        Instantiate(_chargeEfectPrefab, _beamPos.position, _beamPos.rotation);
        _beamEnd = true;
    }
    private void VerticalBeam()
    {
        if (_beamObject!=null)
        {

            _beamObject.transform.eulerAngles -= new Vector3(0.7f, 0, 0);
        }   
    }
    public bool IsNowBeam()
    {
        return _beamObject != null;
    }



}
