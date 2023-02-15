using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AlwaysFaceCamera : MonoBehaviour
{
    private Camera _camera;
    public float offsetX = 0f;
    public Transform offsetTransform;
    public bool executeInEditor = false;

    void Start() {
        _camera = Camera.main;
    }
    
    private void LateUpdate()
    {
        if (Application.IsPlaying(gameObject))
        {
            FaceCamera();
            OffsetTarget();
        }
        else
        {
            if (executeInEditor) {
                FaceCamera();
                OffsetTarget();
            }
        }
    }

    private void FaceCamera() {
        if (_camera == null)
            return;
        transform.forward = new Vector3(_camera.transform.forward.x, transform.forward.y, _camera.transform.forward.z);
        transform.up = new Vector3(transform.up.x, _camera.transform.up.y, _camera.transform.up.z);
    }

    private void OffsetTarget() {
        if (offsetTransform == null)
            return;

        offsetTransform.position = transform.position - transform.forward * offsetX;
    }
}
