using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public string exitName;
    public string toScene;
    public string toExit;
    public Transform spawnPoint;
    public bool locked = false;

    private BoxCollider boxCollider;

    void Start() {
        boxCollider = GetComponent<BoxCollider>();
        UpdateLock();
    }

    void Update() {
        UpdateLock();
    }

    private void UpdateLock() {
        if (boxCollider.isTrigger != !locked)
            boxCollider.isTrigger = !locked;
    }

    public void SetLock(bool value) {
        locked = value;
        UpdateLock();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Player.Instance.toScene = toScene;
        Player.Instance.toExit = toExit;
        SceneController.Instance.LoadAnyScene(toScene);
    }
}
