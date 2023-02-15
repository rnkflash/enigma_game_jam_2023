using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FadeInOut : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeInOutTime = 0.5f;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        FadeOut();

        EventBus<FadeInOutRequest>.Sub(OnRequest);
    }

    void OnDestroy() {
        EventBus<FadeInOutRequest>.Unsub(OnRequest);
    }

    private void OnRequest(FadeInOutRequest message)
    {
        if (message.fadeIn)
            FadeIn();
        else
            FadeOut();
    }

    public void FadeOut() {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
        canvasGroup.DOFade(0f, fadeInOutTime).OnComplete(()=>{
            canvasGroup.blocksRaycasts = false;
            EventBus<FadeInOutCompleted>.Pub(new FadeInOutCompleted());
        });
    }

    public void FadeIn() {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0.0f;
        canvasGroup.DOFade(1.0f, fadeInOutTime).OnComplete(()=>{
            canvasGroup.blocksRaycasts = false;
            EventBus<FadeInOutCompleted>.Pub(new FadeInOutCompleted());
        });
    }
}
