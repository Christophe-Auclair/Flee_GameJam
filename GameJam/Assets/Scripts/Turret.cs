using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Animator anim;
    private Vector3 direction;
    private float cooldown;

    [SerializeField]
    private float range;
    [SerializeField]
    private float cooldownTime;
    [SerializeField]
    private GameObject missile;

    // Start is called before the first frame update
    void Start()
    {
        GameObject targetGO = GameObject.Find("PlayerAsteroid");
        Debug.Assert(targetGO != null);
        if (targetGO != null)
        {
            target = targetGO.transform;
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown--;
        }

        if (target)
        {
            direction = target.position - transform.position;
            direction.Normalize();
            Color col = Color.green;
            float longueurDebug = range;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Player", "Murs"));

            if (hit.collider != null)
            {
                col = Color.blue;
                longueurDebug = hit.distance;
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
                {
                    col = Color.red;
                    if (cooldown <= 0)
                    {
                        Fire();
                    }
                }
            }

            Debug.DrawRay(transform.position, direction * longueurDebug, col);
        }
    }

    public void Fire()
    {
        anim.SetTrigger("Fire");
        cooldown = cooldownTime;
        Instantiate(missile, transform.position, Quaternion.identity);
    }
}
