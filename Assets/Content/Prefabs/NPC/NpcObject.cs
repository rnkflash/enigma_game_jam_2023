using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObject : MonoBehaviour
{
    public TextAsset inkyFile;
    private InteractiveObject interactiveObject;
    public Animator animator;
    
    void Start()
    {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += OnInteract;
    }

    void OnDestroy() {
        interactiveObject.onInteractListeners -= OnInteract;
    }

    private void OnInteract(PlayerInteraction interactor)
    {
        EventBus<NpcDialogStart>.Pub(new NpcDialogStart() {npc = this, inky = inkyFile});
        animator.SetBool("IsTalking", true);
        EventBus<NpcDialogEnd>.Sub(OnFinishTalk);
    }

    private void OnFinishTalk(NpcDialogEnd message)
    {
        if (message.npc == this) {
            animator.SetBool("IsTalking", false);
            EventBus<NpcDialogEnd>.Unsub(OnFinishTalk);
        }
    }

    void Update()
    {
        
    }
}
