using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public LineRenderer laser;
    public Transform laserOrigin;
    public GameObject redDot;
    public LayerMask raycastLayerMask;

    private Camera _camera;

    void Start()
    {
        laser.positionCount = 2;
        _camera = Camera.main;
    }

    void Update()
    {
        var laserRange = 50f;
        laser.SetPosition(0, laserOrigin.position);
        RaycastHit laserHitInfo;
        Ray laserRay = new Ray(laserOrigin.position, laserOrigin.forward);
        var laserHit = Physics.Raycast(laserRay, out laserHitInfo, 100f, raycastLayerMask);
        if (laserHit) {
            laserRange = laserHitInfo.distance;
            redDot.transform.position = laserHitInfo.point;
        } else {
            redDot.transform.position = Vector3.zero;
        }
        laser.SetPosition(1, laserOrigin.position + (laserOrigin.forward * laserRange));
        
    }

    private void LateUpdate()
    {
        redDot.transform.forward = new Vector3(_camera.transform.forward.x, redDot.transform.forward.y, _camera.transform.forward.z);
        redDot.transform.up = new Vector3(redDot.transform.up.x, _camera.transform.up.y, _camera.transform.up.z);
    }
}
