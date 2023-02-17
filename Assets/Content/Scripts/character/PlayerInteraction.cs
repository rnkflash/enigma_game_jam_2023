using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<InteractiveObject> items = new List<InteractiveObject>();
    public InteractiveObject selectedItem;
    private PlayerInputValues inputValues;
    private float useTimeout = 0.5f;
    private float useTimeoutDelta;
    public Transform pickupPoint;
    public InteractiveTags tagsMask;

    void Start()
    {
        selectedItem = null;
        inputValues = GetComponentInParent<PlayerInputValues>();
    }

    void Update()
    {
        SelectClosestItem();
        CheckUseButton();
    }

    private void CheckUseButton() {
        if (inputValues.use && useTimeoutDelta <= 0.0f)
        {
            if (selectedItem!=null) {
                selectedItem.Interact(this);
            }
            useTimeoutDelta = useTimeout;
        }

        if (useTimeout >= 0.0f)
        {
            useTimeoutDelta -= Time.deltaTime;
        }
    }

    private bool CheckTagMask(string tag) {
        var tags = tagsMask.GetUniqueFlags().Select(t=>Enum.GetName(typeof(InteractiveTags), t));
        return tags.Contains(tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckTagMask(other.tag))
            return;
        var item = other.GetComponent<InteractiveObject>();
        items.Add(item);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CheckTagMask(other.tag))
            return;
        var item = other.GetComponent<InteractiveObject>();
        item.Select(false);
        items.Remove(item);
    }

    private void SelectClosestItem() {
        var item = items
            .Where(item=>item.selectable)
            .OrderBy(item=>GetDistPointToLine(transform.position, transform.forward, item.transform.position))
            .FirstOrDefault();
        if (item != null) {
            if (selectedItem != item) {
                selectedItem?.Select(false);
                selectedItem = item;
                selectedItem.Select(true);
            }
        } else {
            if (selectedItem != null) {
                selectedItem.Select(false);
                selectedItem = null;
            }
        }
    }

    private float GetDistPointToLine(Vector3 origin, Vector3 direction, Vector3 point){
        Vector3 point2origin = origin - point;
        Vector3 point2closestPointOnLine = point2origin - Vector3.Dot(point2origin,direction) * direction;
        return point2closestPointOnLine.magnitude;
    }

}
