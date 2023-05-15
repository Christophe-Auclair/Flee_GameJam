using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "SuperNova")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject); 
        }
    }
}
