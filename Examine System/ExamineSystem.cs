using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ExamineSystem : MonoBehaviour
{
    [SerializeField]
    private Animator ExamineObjAnim;
    [SerializeField]
    private GameObject ExamineObj;
    [SerializeField]
    private GameObject[] showedObjects;

    [SerializeField]
    private PostProcessVolume volumeProfile;

    [Range(0.1f, 3)] public float duration = 0.75f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool examinate = false;
    private GameObject currentExamineObj;
    public ExamineObject currentPickUp { get; set; }

    #region Singleton
    public static ExamineSystem m_ExamineSystem { get; set; }
    private void Awake()
    {
        if (m_ExamineSystem == null)
            m_ExamineSystem = this;
    }
    #endregion

    public void Examine() //начало изучения
    {
        if (!currentPickUp) return;

        if (!examinate)
        {
            examinate = true;
            StartExamine();
        }
        else
        {
            examinate = false;
            StartDone();
        }
    }

    private void StartExamine()
    {
        currentExamineObj = currentPickUp.gameObject;
        showedObjects[currentPickUp.objID].SetActive(true);
        volumeProfile.gameObject.SetActive(true);

        MenuUI.m_MenuUI.ItemText(currentPickUp.itemName);
        MenuUI.m_MenuUI.ShowPickupText(false);
        currentExamineObj.SetActive(false);
        WeightIn(duration, ShowObject);
    }

    private void ShowObject()
    {
        ExamineObj.SetActive(true);
        ExamineObjAnim.Play(StaticStrings.ExamineStart);
    }

    public void StartDone()     //заверешние
    {
        ExamineObjAnim.Play(StaticStrings.ExamineDone);
        MenuUI.m_MenuUI.HideItemText();
        currentPickUp = null;
        WeightOut(duration + 2f, AfterDone);
    }

    private void AfterDone()
    {
        volumeProfile.gameObject.SetActive(false);
        currentExamineObj = null;
        ExamineObj.SetActive(false);

        foreach (var obj in showedObjects)
            obj.SetActive(false);
    }

    public void WeightIn(float lenght = 0, Action onFinish = null)
    {
        StopAllCoroutines();
        StartCoroutine(PostWeightPlus(lenght, onFinish));
    }

    public void WeightOut(float lenght = 0, Action onFinish = null)
    {
        StopAllCoroutines();
        StartCoroutine(PostWeightMinus(lenght, onFinish));
    }

    IEnumerator PostWeightPlus(float lenght = 0, Action onFinish = null)
    {
        float d = 0;
        float t = 0;
        float dur = lenght <= 0 ? duration : lenght;
        while (d < 1)
        {
            d += Time.deltaTime / dur;
            t = fadeCurve.Evaluate(d);
            volumeProfile.weight = t;
            yield return null;
        }

        onFinish?.Invoke();
    }
    IEnumerator PostWeightMinus(float lenght = 0, Action onFinish = null)
    {
        float d = 1;
        float t = 0;
        float dur = lenght <= 0 ? duration : lenght;
        while (d > 0.5)
        {
            d += Time.deltaTime / dur;
            t = fadeCurve.Evaluate(d);
            volumeProfile.weight = 1 - t;
            yield return null;
        }

        onFinish?.Invoke();
    }
}
