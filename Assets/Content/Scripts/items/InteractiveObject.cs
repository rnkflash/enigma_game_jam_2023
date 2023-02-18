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
    public event OnInteract onStartSelectListeners;
    public event OnInteract onStopSelectListeners;

    void Start() {
        selectUI = GetComponentInChildren<SelectUI>();
    }

    public void Select(bool selected, PlayerInteraction player) {
        if (selected) {
            selectUI?.Appear();
            onStartSelectListeners?.Invoke(player);
        }
        else {
            selectUI?.Disappear();
            onStopSelectListeners?.Invoke(player);
        }
    }

    public void Interact(PlayerInteraction player) {
        onInteractListeners?.Invoke(player);
    }
}