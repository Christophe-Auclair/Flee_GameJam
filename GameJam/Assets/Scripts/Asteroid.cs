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
        initialRotation = Random.Range(0, 360);
        rotationSpeed = Random.Range(0, 2f);
        transform.Rotate(new Vector3(0, 0, initialRotation));
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        //if (layerName == "Walls" || layerName == "Planets" || layerName == "SuperNova" || layerName == "Player" || layerName == "Ships")
        //{
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        //}
    }
}
