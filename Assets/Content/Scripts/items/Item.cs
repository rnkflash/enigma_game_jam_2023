using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;
    private InteractiveObject interactiveObject;

    void Start() {
        interactiveObject = GetComponentInChildren<InteractiveObject>();
        interactiveObject.onInteractListeners += PickUp;
    }

    void OnDestroy() {
        interactiveObject.onInteractListeners -= PickUp;
    }

    public void PickUp(PlayerInteraction interactor) {
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
}
