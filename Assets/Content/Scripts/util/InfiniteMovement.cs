using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMovement : MonoBehaviour
{
    public Vector3 movement = Vector3.zero;

    void FixedUpdate()
    {
        transform.position += movement;
    }
}
