using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningShip : MonoBehaviour
{
    [SerializeField]
    private string nomDeLaCible;
    [SerializeField]
    private float longueurRayon;
    [SerializeField]
    private float vitesse = 2.0f;
    [SerializeField]
    private GameObject explosion;

    private Transform cible;
    private Animator anim;
    private Rigidbody2D rig;
    private Vector3 direction;
    private Vector3 mouvement;
    private bool estEnChasse = false;
    private bool alive = true;
    private float initialRotation;

    void Start()
    {
        GameObject go = GameObject.Find(nomDeLaCible);
        Debug.Assert(go != null);
        if (go != null)
        {
            cible = go.transform;
        }

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        estEnChasse = false;

        initialRotation = Random.Range(0, 360);
        transform.Rotate(new Vector3(0, 0, initialRotation));
    }

    private void FixedUpdate()
    {
        if (cible)
        {
            direction = cible.position - transform.position;
            direction.Normalize();
            Color col = Color.green;
            float longueurDebug = longueurRayon;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, longueurRayon, LayerMask.GetMask("Player", "Murs"));
            if (alive)
            {
                if (hit.collider != null)
                {
                    //J'ai touché quelque chose!
                    col = Color.blue;
                    longueurDebug = hit.distance;
                    if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
                    {
                        if (!estEnChasse) //OnEnterState
                        {
                            StopAllCoroutines();
                        }
                        estEnChasse = true;
                        col = Color.red;
                        mouvement = direction;
                    }
                    else
                    {
                        if (estEnChasse) //OnEnterState
                        {
                            mouvement = Vector2.zero;
                        }
                        estEnChasse = false;
                    }
                }
                else
                {
                    if (estEnChasse) //OnEnterState
                    {
                        mouvement = Vector2.zero;
                    }
                    estEnChasse = false;
                }
                rig.velocity = mouvement * vitesse;
                Debug.DrawRay(transform.position, direction * longueurDebug, col);
            }
            if (estEnChasse)
            {
                anim.SetBool("EngineOn", true);
                RotateTowards(cible.position);
            }
            else
            {
                anim.SetBool("EngineOn", false);
            }
        }
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

        if (layerName == "SuperNova" || layerName == "Planets" || layerName == "Missiles")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}