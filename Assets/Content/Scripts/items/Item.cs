using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public SelectUI selectUI;

    public void Select(bool selected) {
        if (selected)
            selectUI.Appear();
        else
            selectUI.Disappear();
    }
}
