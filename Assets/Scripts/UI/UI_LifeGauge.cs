using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_LifeGauge : MonoBehaviour
{
    [SerializeField]
    LifePointBase LifePoint = null;

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

        GaugeAnimation();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void GaugeAnimation()
    {
        _gaugeImage.fillAmount = 0.5f;
        DOTween.To(() => _gaugeBGImage.fillAmount, num => _gaugeBGImage.fillAmount = num, 0.5f, _animationTime);
    }
}
