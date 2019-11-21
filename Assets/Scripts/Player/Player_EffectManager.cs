using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EffectManager : MonoBehaviour
{
    public enum EffectType
    {
        BurnerFire,
        SparkFire,
        SpeedLine,
        SlashTrail,
        NUM
    }

    [SerializeField]
    private ParticleSystem[] _ParticleSystemList = new ParticleSystem[(int)EffectType.NUM];

    public void ParticlePlay(EffectType effectType)
    {
        if (_ParticleSystemList[(int)effectType] == null)
        {
            Debug.Log("◆Player_EffectManager : NULL");
            return;
        }

        _ParticleSystemList[(int)effectType].Play();
    }
    public void ParticlePlay(int effectType)
    {
        if (_ParticleSystemList[(int)effectType] == null)
        {
            Debug.Log("◆Player_EffectManager : NULL");
            return;
        }

        _ParticleSystemList[(int)effectType].Play();
    }

    public void ParticleStop(EffectType effectType)
    {
        if (_ParticleSystemList[(int)effectType] == null)
        {
            Debug.Log("◆Player_EffectManager : NULL");
            return;
        }

        _ParticleSystemList[(int)effectType].Stop();
    }
    public void ParticleStop(int effectType)
    {
        if (_ParticleSystemList[(int)effectType] == null)
        {
            Debug.Log("◆Player_EffectManager : NULL");
            return;
        }

        _ParticleSystemList[(int)effectType].Stop();
    }

    public void AllParticlePlay()
    {
        for (int i = 0; i < _ParticleSystemList.Length; i++)
        {
            ParticlePlay(i);
        }
    }
    public void AllParticleStop()
    {
        for (int i = 0; i < _ParticleSystemList.Length; i++)
        {
            ParticleStop(i);
        }
    }
}
