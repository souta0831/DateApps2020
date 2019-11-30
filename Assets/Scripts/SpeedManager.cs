using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class SpeedManager : MonoBehaviour
{
    //private float _scrollSpeed=0;
    ReactiveProperty<float> _scrollSpeed = new ReactiveProperty<float>(0);
    public IReadOnlyReactiveProperty<float> speedProperty
    {
        get { return _scrollSpeed; }
    }
    public void SetSpeed(float vaule)
    {
        _scrollSpeed.Value = vaule;
    }
}