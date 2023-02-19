using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamSceneFinish : MonoBehaviour
{
    public CommentDialogueSO comment;
    private bool playComment = false;

    void Awake()
    {
        if (!Player.Instance.dreamScenePassed) {
            Player.Instance.dreamScenePassed = true;
            Player.Instance.cosmonaft = false;
            playComment = true;
        }
        
    }

    void Update() {
        if (playComment) {
            EventBus<CommentDialogStart>.Pub(new CommentDialogStart() {payload = comment});
            playComment = false;
        }

        gameObject.SetActive(false);
    }


}
