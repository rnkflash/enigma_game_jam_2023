using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderUpdater : MonoBehaviour
{
    public float updateTime = 0.25f;
    float time = 0;
    SkinnedMeshRenderer meshRenderer;
    MeshCollider meshCollider;

    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= updateTime)
        {
            time = 0;
            UpdateCollider();
        }
    }

    public void UpdateCollider()
    {
        Mesh mesh = new Mesh();
        meshRenderer.BakeMesh(mesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}
