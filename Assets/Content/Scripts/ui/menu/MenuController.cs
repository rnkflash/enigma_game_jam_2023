using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    
    public void OnButtonClicked(int i) {
        if (i == 0) {

            OnStartClicked();
        }
        else
            SceneController.Instance.LoadMainMenu();
    }


    public void OnStartClicked() {
        EventBus<FadeInOutRequest>.Pub(new FadeInOutRequest() {fadeIn = true});
        EventBus<FadeInOutCompleted>.Sub(OnFadeCompleted);
    }

    private void OnFadeCompleted(FadeInOutCompleted message)
    {
        EventBus<FadeInOutCompleted>.Unsub(OnFadeCompleted);
        SceneController.Instance.LoadIntro();
    }
}
