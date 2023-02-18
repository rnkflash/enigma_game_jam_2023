using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Item : MonoBehaviour, IConfirmDialogInitiator
{
    public ItemData item;
    public int amount = 1;
    private InteractiveObject interactiveObject;
    private PlayerInteraction interactor;
    public GameObject model;
    public bool pickedUp = false;

    void Start() {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += Interact;

        SetPickedUp(pickedUp);
    }

    void OnDestroy() {
        interactor = null;
        interactiveObject.onInteractListeners -= Interact;
    }

    private void Interact(PlayerInteraction interactor) {
        this.interactor = interactor;
        EventBus<ConfirmDialogStart>.Pub(new ConfirmDialogStart() {
            initiator = this,
            question = "Podobrat " + item.name + "?",
            yes = "Da",
            no = "Niet"
        });
    }

    private void PickUp() {
        pickedUp = true;
        interactiveObject.selectable = false;
        model.transform.DOScale(0f, 0.25f);
        model.transform.DOMove(interactor.pickupPoint.position, 0.25f).OnComplete(()=> {
            Player.Instance.AddItem(item, amount);
            model.SetActive(false);
            DOTween.Kill(model);
        });
    }

    public void ConfirmDialog(bool result)
    {
        if (result)
            PickUp();
    }

    public void SetPickedUp(bool value) {
        pickedUp = value;
        interactiveObject.selectable = !value;
        model.SetActive(!value);
    }
}
