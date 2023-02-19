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

    public GameObject[] switchLayerObjects;
    public string switchToLayer = "Select";
    private int[] savedLayers;

    void Start() {
        selectUI = GetComponentInChildren<SelectUI>();

        savedLayers = new int[switchLayerObjects.Length];
    }

    public void Select(bool selected, PlayerInteraction player) {
        if (selected) {
            selectUI?.Appear();

            var index = 0;
            foreach (var item in switchLayerObjects)
            {
                if (item != null) {
                    savedLayers[index++] = item.layer;
                    item.layer = LayerMask.NameToLayer(switchToLayer);
                }
            }

            onStartSelectListeners?.Invoke(player);
        }
        else {

            var index = 0;
            foreach (var item in switchLayerObjects)
            {
                if (item != null)
                    item.layer = savedLayers[index++];
            }

            selectUI?.Disappear();
            onStopSelectListeners?.Invoke(player);
        }
    }

    public void Interact(PlayerInteraction player) {
        onInteractListeners?.Invoke(player);
    }
}