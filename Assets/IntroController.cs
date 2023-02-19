using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnStartClicked() {
        EventBus<FadeInOutRequest>.Pub(new FadeInOutRequest() {fadeIn = true});
        EventBus<FadeInOutCompleted>.Sub(OnFadeCompleted);
    }

    private void OnFadeCompleted(FadeInOutCompleted message)
    {
        EventBus<FadeInOutCompleted>.Unsub(OnFadeCompleted);
        SceneController.Instance.LoadGameplay();
    }
}
