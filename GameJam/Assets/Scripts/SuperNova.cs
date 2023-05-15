using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNova : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void FixedUpdate()
    {
        float newX = transform.position.x + speed;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
