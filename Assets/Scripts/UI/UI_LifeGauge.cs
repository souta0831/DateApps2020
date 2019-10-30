using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using System; // .NET 4.xモードで動かす場合は必須

public class UI_LifeGauge : MonoBehaviour
{
    [SerializeField]
    LifePointBase _lifePoint = null;

    [SerializeField]
    private Image _gaugeImage = null;
    [SerializeField]
    private Image _gaugeBGImage = null;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _fillAmountMax = 0.85f;

    [SerializeField]
    private Color _gaugeColor;

    [SerializeField]
    private float _animationTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _gaugeImage.fillAmount = _fillAmountMax;
        _gaugeBGImage.fillAmount = _fillAmountMax;

        _gaugeImage.color = _gaugeColor;

        _lifePoint.OnPointChanged.DistinctUntilChanged().Subscribe(_count =>
        {
            GaugeAnimation();
        });
    }

    private void GaugeAnimation()
    {
        _gaugeImage.fillAmount = _fillAmountMax * ((float)_lifePoint.GetNowPoint() / (float)_lifePoint.GetMaxPoint());
        DOTween.To(() => _gaugeBGImage.fillAmount, num => _gaugeBGImage.fillAmount = num, _gaugeImage.fillAmount, _animationTime);
    }
}
