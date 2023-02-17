using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentDialogueObject: MonoBehaviour {
    
    private InteractiveObject interactiveObject;
    public CommentDialogueSO commentDialog;

    void Start() {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += Interact;
    }

    void OnDestroy() {
        interactiveObject.onInteractListeners -= Interact;
    }

    void Interact(PlayerInteraction interactor) {
        EventBus<CommentDialogStart>.Pub(new CommentDialogStart() {payload = commentDialog});
    }
}