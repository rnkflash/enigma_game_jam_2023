using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DreamSceneInit : MonoBehaviour
{
    
    public Animator wify;

    public Rigidbody wifeBody;
    public Transform targetToThrowWife;

    void Awake()
    {
        Player.Instance.dreamScenePassed = false;
        Player.Instance.cosmonaft = false;

        EventBus<TriggerWasSet>.Sub(OnTrigger);

        wifeBody.isKinematic = true;
        
    }

    void OnDestroy() {
        EventBus<TriggerWasSet>.Unsub(OnTrigger);
    }

    private void OnTrigger(TriggerWasSet message)
    {
        if (message.trigger == "wife_jump") {
            wify.SetBool("Falling", true);
            wifeBody.isKinematic = false;
            wifeBody.transform.DOMove(targetToThrowWife.position, 1.25f).SetDelay(0.85f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
