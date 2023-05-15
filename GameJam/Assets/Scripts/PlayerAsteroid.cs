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
    private GameObject explosion;

    private Rigidbody2D rig;
    private Vector2 movement;
    //private Animator anim;
    private bool alive;
    public bool control;
    private bool facingRight;
    private FacingDirection facingDirection;

    public enum FacingDirection
    {
        Left,
        Right,
        Down,
        Up
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        facingDirection = FacingDirection.Right;
        alive = true;
        control = true;
    }

    private void GetInput()
    {
        Debug.Log("in iput");
        if (control && alive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyUp(KeyCode.Space))
            {
                //anim.SetTrigger("Attacking");
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                //anim.SetTrigger("Startled");
            }

            //anim.SetFloat("Vertical", movement.y);
            //anim.SetFloat("Horizontal", movement.x);

            if (movement.x > 0)
            {
                facingDirection = FacingDirection.Right;
                //Flip(0);
            }
            else if (movement.x < 0)
            {
                facingDirection = FacingDirection.Left;
                //Flip(1);
            }
            else if (movement.y > 0)
            {
                facingDirection = FacingDirection.Up;
            }
            else if (movement.y < 0)
            {
                facingDirection = FacingDirection.Down;
            }

            //anim.SetFloat("Up", facingDirection == FacingDirection.Up ? 1 : 0);
            //anim.SetFloat("Down", facingDirection == FacingDirection.Down ? 1 : 0);
            //anim.SetFloat("Left", facingDirection == FacingDirection.Left ? 1 : 0);
            //anim.SetFloat("Right", facingDirection == FacingDirection.Right ? 1 : 0);
            //anim.SetFloat("FacingRight", facingRight == true ? 1 : 0);
            //anim.SetFloat("FacingLeft", facingRight == false ? 1 : 0);
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
    }

    private void Move()
    {
        rig.velocity = movement * moveSpeed;
        rig.velocity = rig.velocity.normalized * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        this.transform.Rotate(0, 0 ,0.1f);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Walls" || layerName == "Enemies" || layerName == "SuperNova")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            await Task.Delay(1000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
