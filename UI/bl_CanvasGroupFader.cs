using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class bl_CanvasGroupFader : MonoBehaviour
{
    [Range(0.1f, 3)] public float duration = 0.75f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Range(0.1f, 5)] public float fadeTime = 0.75f;
    [SerializeField] private bool fadeOnStart;

    public static bl_CanvasGroupFader m_canvasGrouper;
    private void Awake()
    {
        if (m_canvasGrouper == null)
            m_canvasGrouper = this;

        if (fadeOnStart)
            FadeIn(fadeTime);
    }

    private CanvasGroup m_canvasGroup;
    private CanvasGroup canvasGroup
    {
        get
        {
            if (m_canvasGroup == null) m_canvasGroup = GetComponent<CanvasGroup>();
            return m_canvasGroup;
        }
    }

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha; if (alpha > 0) gameObject.SetActive(true);
    }

    public void JustFade()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(DoFadeOut(2f));
    }

    public void FadeIn(float lenght = 0, Action onFinish = null)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(DoFadeIn(lenght, onFinish));
    }

    public void FadeOut(float lenght = 0, Action onFinish = null)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(DoFadeOut(lenght, onFinish));
    }

    IEnumerator DoFadeIn(float lenght = 0, Action onFinish = null)
    {
        float d = 0;
        float t = 0;
        float dur = lenght <= 0 ? duration : lenght;
        while (d < 1)
        {
            d += Time.deltaTime / dur;
            t = fadeCurve.Evaluate(d);
            canvasGroup.alpha = t;
            yield return null;
        }
        onFinish?.Invoke();
    }
    IEnumerator DoFadeOut(float lenght = 0, Action onFinish = null)
    {
        float d = 0;
        float t = 0;
        float dur = lenght <= 0 ? duration : lenght;
        while (d < 1)
        {
            d += Time.deltaTime / dur;
            t = fadeCurve.Evaluate(d);
            canvasGroup.alpha = 1 - t;
            yield return null;
        }
        onFinish?.Invoke();
        gameObject.SetActive(false);
    }
}