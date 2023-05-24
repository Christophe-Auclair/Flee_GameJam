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
    private float boostDecelaration;
    [SerializeField]
    private float boostDecelerationRate;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject endExplosion;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private int hp = 3;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rig;
    private Vector2 movement;
    public bool control = false;
    public bool end = false;
    private bool boosted;
    private bool boostEnded = true;
    private int boostedFramesRemaining;

    private Color flickerColor = Color.red;
    private float flickerDuration = 0.1f;
    private int flickerCount = 5;
    private bool invulnerable = false;

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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            await Task.Delay(3000);
        }
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

            if (boosted)
            {
                boostedFramesRemaining -= 1;
                if (boostedFramesRemaining <= 0)
                {
                    boosted = false;
                }
            }
            else if (!boosted && !boostEnded)
            {
                boostDecelaration -= boostDecelerationRate * Time.deltaTime;
                Debug.Log(boostDecelaration);
                if (boostDecelaration <= maxSpeed)
                {
                    boostEnded = true;
                }
                rig.velocity = Vector2.ClampMagnitude(rig.velocity, boostDecelaration);
            }
            else if (!boosted && boostEnded)
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
            if (!invulnerable)
            {
                this.hp -= 1;
                HealthBar.instance.SetHealth(this.hp);
                StartCoroutine(FlickerSprite());
            }
        }
        if (layerName == "SuperNova")
        {
            this.hp -= 3;
            HealthBar.instance.SetHealth(this.hp);
        }
    }

    private IEnumerator FlickerSprite()
    {
        invulnerable = true;
        for (int i = 0; i < flickerCount; i++)
        {
            spriteRenderer.color = flickerColor;
            yield return new WaitForSeconds(flickerDuration);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
        }
        invulnerable = false;
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Portal")
        {
            control = false;
            boosted = true;
            end = true;
            GameObject portal = GameObject.Find("Portal");
            Vector3 direction = portal.transform.position - transform.position;
            direction.Normalize();
            rig.velocity = new Vector2(0f, 0f);
            rig.AddForce(new Vector2((direction.x * 15f), (direction.y * 15f)), ForceMode2D.Impulse);
            await Task.Delay(1000);
            gameManager.FadeOutGameplay();
            Destroy(this.gameObject);
            await Task.Delay(1500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }   
        else if (layerName == "Boost")
        {
            rig.AddForce(new Vector2((movement.x * (moveSpeed * 1.5f)), (movement.y * (moveSpeed * 0.75f))), ForceMode2D.Impulse);
            boosted = true;
            boostEnded = false;
            boostedFramesRemaining = 60;
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
            await Task.Delay(1000);
            gameManager.FadeOutGameplay();
            await Task.Delay(1500);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
