using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData item;
    private SelectUI selectUI;

    void Start() {
        selectUI = GetComponentInChildren<SelectUI>();
    }

    public void Select(bool selected) {
        if (selected)
            selectUI.Appear();
        else
            selectUI.Disappear();
    }

    public void PickUp() {
        Debug.Log($"Picked up {item.name}. {item.description}");
    }
}
