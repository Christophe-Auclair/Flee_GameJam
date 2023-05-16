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
    private GameManager gameManager;
    [SerializeField]
    private int hp = 2;

    private Rigidbody2D rig;
    private Vector2 movement;
    public bool control = false;
    private bool boosted;
    private int boostedFramesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Spawn();
        HealthBar.instance.SetMaxHealth(this.hp);
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
        rig.AddForce(new Vector2((movement.x * moveSpeed), (movement.y * moveSpeed)), ForceMode2D.Force);
        if (!boosted)
        {
            rig.velocity = Vector2.ClampMagnitude(rig.velocity, maxSpeed);
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
        Debug.Log(hp);

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

        if (layerName == "Walls" || layerName == "Ships" || layerName == "Asteroids" || layerName == "Planets")
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }   
        else if (layerName == "Boost")
        {
            rig.AddForce(new Vector2((movement.x * (moveSpeed * 1.5f)), (movement.y * (moveSpeed * 0.75f))), ForceMode2D.Impulse);
            boosted = true;
            boostedFramesRemaining = 130;
        }
    }
}
