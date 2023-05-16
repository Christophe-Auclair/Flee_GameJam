using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerAsteroid : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject endExplosion;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private int hp = 3;

    private Rigidbody2D rig;
    private Vector2 movement;
    public bool control = false;
    public bool end = false;
    private bool boosted;
    private int boostedFramesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Spawn();
        HealthBar.instance.SetMaxHealth(this.hp);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            rig.AddForce(new Vector2(5, 0f), ForceMode2D.Impulse);
        }
    }

    public async void Spawn()
    {
        await Task.Delay(3000);
        control = true;
    }

    private void GetInput()
    {
        if (control)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
    }

    private void Move()
    {
        if (!end)
        {
            rig.AddForce(new Vector2((movement.x * moveSpeed), (movement.y * moveSpeed)), ForceMode2D.Force);
            if (!boosted)
            {
                rig.velocity = Vector2.ClampMagnitude(rig.velocity, maxSpeed);
            }
        }
    }

    void Update()
    {
        GetInput();
        CheckIfDead();
    }

    private void FixedUpdate()
    {
        Move();
        this.transform.Rotate(0, 0, 1f);
        if (boosted)
        {
            boostedFramesRemaining -= 1;
            if (boostedFramesRemaining <= 0)
            {
                boosted = false;
            }
        }
    }

    public async void CheckIfDead()
    {
        if (this.hp <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            gameManager.FadeOutGameplay();
            await Task.Delay(1500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Walls" || layerName == "Ships" || layerName == "Asteroids" || layerName == "Planets" || layerName == "Missiles")
        {
            this.hp -= 1;
            HealthBar.instance.SetHealth(this.hp);
        }
        if (layerName == "SuperNova")
        {
            this.hp -= 2;
            HealthBar.instance.SetHealth(this.hp);
        }
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Portal")
        {
            gameManager.FadeOutGameplay();
            await Task.Delay(1500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }   
        else if (layerName == "Boost")
        {
            rig.AddForce(new Vector2((movement.x * (moveSpeed * 1.5f)), (movement.y * (moveSpeed * 0.75f))), ForceMode2D.Impulse);
            boosted = true;
            boostedFramesRemaining = 130;
        }   
        else if (layerName == "MotherShip")
        {
            control = false;
            boosted = true;
            end = true;
            GameObject motherShip = GameObject.Find("MotherShip");
            Vector3 direction = motherShip.transform.position - transform.position;
            direction.Normalize();
            rig.velocity = new Vector2(0f, 0f);
            rig.AddForce(new Vector2((direction.x * 15f), (direction.y * 15f)), ForceMode2D.Impulse);
        }
        else if (layerName == "MotherShipHitBox")
        {
            Instantiate(endExplosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            GameObject motherShip = GameObject.Find("MotherShip");
            Destroy(motherShip);
            await Task.Delay(3000);
            gameManager.FadeOutGameplay();
            await Task.Delay(1500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
