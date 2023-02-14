using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    private Camera _camera;
    public float offsetX = 0f;
    public Transform offsetTransform;

    void Start() {
        _camera = Camera.main;
    }
    
    private void LateUpdate()
    {
        transform.forward = new Vector3(_camera.transform.forward.x, transform.forward.y, _camera.transform.forward.z);
        transform.up = new Vector3(transform.up.x, _camera.transform.up.y, _camera.transform.up.z);
        if (offsetTransform != null)
            offsetTransform.position = transform.position + transform.forward * offsetX;
    }
}
