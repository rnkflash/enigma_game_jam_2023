using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{

    private bool waitingInput;

    void Start()
    {
        waitingInput = false;
        EventBus<FadeInOutCompleted>.Sub(OnFadeCompleted1);
    }

    private void OnFadeCompleted1(FadeInOutCompleted message)
    {
        waitingInput = true;
        EventBus<FadeInOutCompleted>.Unsub(OnFadeCompleted1);
    }

    void Update()
    {
        if (!waitingInput)
            return;
        if (Input.anyKey)
        {
            OnStartClicked();
            waitingInput = false;
        }
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
