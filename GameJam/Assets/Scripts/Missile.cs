using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Vector3 direction;
    private Transform target;
    private Rigidbody2D rig;
    private GameObject targetGO;
    [SerializeField]
    private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        targetGO = GameObject.Find("PlayerAsteroid");
        target = targetGO.transform;
        direction = target.position - transform.position;
        direction.Normalize();
        RotateTowards(target.position);
        rig.AddForce(new Vector2((direction.x * 10f), (direction.y * 10f)), ForceMode2D.Impulse);
    }

    private void RotateTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var offset = 90f;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        
    }
}
