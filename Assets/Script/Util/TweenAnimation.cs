using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenAnimation : MonoBehaviour
{

    public bool autoPlay = false;
    [Space(10)]
    public TweenType tweenType = TweenType.Move;
    public Vector3 begin, end;
    [Space(10)]
    public float useTime = 1;
    public float useTimeRandomRange = 0;
    public float delayTime = 0;
    public float dalayRandomRange = 0;
    public LoopType looptype;
    public int loopTimes = 1;
    public Ease easeType = Ease.Linear;


    void OnEnable()
    {
        if (autoPlay)
            Play();
    }

    void OnDisable()
    {
        DOTween.Kill("TweenAnimation" + GetInstanceID());
    }

    public void Play()
    {
        DOTween.Kill("TweenAnimation" + GetInstanceID());

        if (tweenType == TweenType.Move)
        {
            transform.localPosition = begin;
            transform.DOLocalMove(end, useTime + Random.Range(-useTimeRandomRange, useTimeRandomRange)).SetDelay(delayTime + Random.Range(-dalayRandomRange, dalayRandomRange)).SetLoops(loopTimes, looptype).SetEase(easeType).SetId("TweenAnimation" + GetInstanceID());
        }
        else if (tweenType == TweenType.Rotate)
        {
            transform.localEulerAngles = begin;
            transform.DOLocalRotate(end, useTime + Random.Range(-useTimeRandomRange, useTimeRandomRange)).SetRelative(true).SetDelay(delayTime + Random.Range(-dalayRandomRange, dalayRandomRange)).SetLoops(loopTimes, looptype).SetEase(easeType).SetId("TweenAnimation" + GetInstanceID());
        }
        else if (tweenType == TweenType.Scale)
        {
            transform.localScale = begin;
            transform.DOScale(end, useTime + Random.Range(-useTimeRandomRange, useTimeRandomRange)).SetDelay(delayTime + Random.Range(-dalayRandomRange, dalayRandomRange)).SetLoops(loopTimes, looptype).SetEase(easeType).SetId("TweenAnimation" + GetInstanceID());
        }
    }

    public void Stop()
    {
        transform.DOKill();
    }

    public enum TweenType
    {
        Move,
        Rotate,
        Scale
    }
}
