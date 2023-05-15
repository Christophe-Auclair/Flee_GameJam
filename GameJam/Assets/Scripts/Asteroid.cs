using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    private float rotationSpeed;
    private float initialRotation;


    void Start()
    {
        initialRotation = Random.Range(0, 1);
        rotationSpeed = Random.Range(0, 0.3f);
        transform.position = new Vector3(transform.position.x, transform.position.y, initialRotation);
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Walls" || layerName == "Enemies" || layerName == "SuperNova")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
