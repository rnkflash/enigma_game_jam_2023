using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreDialogueObject: MonoBehaviour {
    
    private InteractiveObject interactiveObject;
    public LoreDialogueSO loreDialog;

    void Start() {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += Interact;
    }

    void OnDestroy() {
        interactiveObject.onInteractListeners -= Interact;
    }

    void Interact(PlayerInteraction interactor) {
        EventBus<LoreDialogStart>.Pub(new LoreDialogStart() {payload = loreDialog});
    }
}