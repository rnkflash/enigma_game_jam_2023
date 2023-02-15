using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractiveObject : MonoBehaviour
{
    private SelectUI selectUI;
    public bool selectable = true;

    public delegate void OnInteract(PlayerInteraction interactor);
    public event OnInteract onInteractListeners;

    void Start() {
        selectUI = GetComponentInChildren<SelectUI>();
    }

    public void Select(bool selected) {
        if (selected)
            selectUI.Appear();
        else
            selectUI.Disappear();
    }

    public void Interact(PlayerInteraction player) {
        onInteractListeners?.Invoke(player);
    }
}