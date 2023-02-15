using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public ItemData item;
    private SelectUI selectUI;
    public bool selectable = true;

    void Start() {
        selectUI = GetComponentInChildren<SelectUI>();
    }

    public void Select(bool selected) {
        if (selected)
            selectUI.Appear();
        else
            selectUI.Disappear();
    }

    public void PickUp(Transform whoPickedUp) {
        Debug.Log($"Picked up {item.itemName}. {item.description}");
        Select(false);
        selectable = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        transform.DOScale(0f, 0.25f);
        transform.DOMove(whoPickedUp.position, 0.25f).OnComplete(()=>Destroy(gameObject));
        //transform.DOJump(whoPickedUp.position, 0f, 1, 0.5f).OnComplete(()=>Destroy(gameObject));
    }
}
