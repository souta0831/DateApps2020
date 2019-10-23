using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TransitionScreen : SingletonMonoBehaviour<TransitionScreen>
{
    private enum FadeType
    {
        FadeIn,
        FadeOut
    }

    [SerializeField] private Image m_image = null;
    [SerializeField] private Color m_fadeColor = Color.black;

    [SerializeField] private float m_fadeDuration = 0.5f;

    private bool m_isFade = false;

    public bool IsFade { get { return m_isFade; } }

    public void StartFadeIn()
    {
        FadeInternal(1.0f, FadeType.FadeIn);
    }

    public void StartFadeOut()
    {
        FadeInternal(0.0f, FadeType.FadeOut);
    }

    private void FadeInternal(float tagetAlpha, FadeType fadeType)
    {
        m_isFade = true;
        m_image.enabled = true;
        Debug.Log(fadeType.ToString());

        Sequence seq = DOTween.Sequence();

        seq.Append(DOTween.ToAlpha(() => m_image.color, color => m_image.color = color, tagetAlpha, m_fadeDuration));

        seq.OnComplete(() =>
        {
            Color color = m_fadeColor;

            color.a = tagetAlpha;
            m_image.color = color;

            if (fadeType == FadeType.FadeOut)
            {
                m_image.enabled = false;
            }

            m_isFade = false;
        });
    }

    public IEnumerator LoadSceneWait(string scenePath)
    {
        StartFadeIn();
        yield return new WaitWhile(() => IsFade);
        SceneManager.LoadScene(scenePath);
        StartFadeOut();
        yield return new WaitWhile(() => IsFade);
    }

    public IEnumerator LoadSceneWait(int scenePath)
    {
        StartFadeIn();
        yield return new WaitWhile(() => IsFade);
        SceneManager.LoadScene(scenePath);
        StartFadeOut();
        yield return new WaitWhile(() => IsFade);
    }
}
