using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class InitialExplosition : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float force, radius;

    void Start()
    {
        SpawnInitialExplosion();
    }

    public async void SpawnInitialExplosion()
    {
        await Task.Delay(3000);
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(exp, 3);
        Knockback();
    }

    public void Knockback()
    {
        Collider2D[] inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D nearby in inExplosionRadius)
        {
            Rigidbody2D rig = nearby.GetComponent<Rigidbody2D>();
            if (rig != null)
            {
                Vector2 distance = rig.transform.position - transform.position;
                if (distance.magnitude > 0)
                {
                    float explosionForce = force / distance.magnitude;
                    rig.AddForce(distance.normalized * explosionForce);
                }
            }
        }
    }

    public void DrawExplosionRange()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
