using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    public Item selectedItem;
    private PlayerInputValues inputValues;
    private float useTimeout = 0.5f;
    private float useTimeoutDelta;

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
                selectedItem.PickUp();
                Destroy(selectedItem.gameObject);
                items.Remove(selectedItem);
                selectedItem = null;
            }
            useTimeoutDelta = useTimeout;
        }

        if (useTimeout >= 0.0f)
        {
            useTimeoutDelta -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Item")
            return;
        var item = other.GetComponent<Item>();
        items.Add(item);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Item")
            return;
        var item = other.GetComponent<Item>();
        item.Select(false);
        items.Remove(item);
    }

    private void SelectClosestItem() {
        var item = items.OrderBy(item=>GetDistPointToLine(transform.position, transform.forward, item.transform.position)).FirstOrDefault();
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
