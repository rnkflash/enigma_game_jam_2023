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

    void Start() {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += Interact;
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
        interactiveObject.selectable = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        transform.DOScale(0f, 0.25f);
        transform.DOMove(interactor.pickupPoint.position, 0.25f).OnComplete(()=> {
            Player.Instance.AddItem(item, amount);
            Destroy(gameObject);
        });
    }

    public void ConfirmDialog(bool result)
    {
        if (result)
            PickUp();
    }
}
