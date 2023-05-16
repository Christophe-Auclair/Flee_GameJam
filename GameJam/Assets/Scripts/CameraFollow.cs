using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    private float zDistance = -10.0f;
    private float yDistance = 0.0f;

    void Update()
    {
        if (target)
        {
            transform.position = new Vector3(target.transform.position.x, yDistance, zDistance);
        }
    }
}
