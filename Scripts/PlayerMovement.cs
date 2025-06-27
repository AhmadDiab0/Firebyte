using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        // Refrence the Rigidbody2D and Animator components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocityY);

        // Flip the player sprite based on horizontal input
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3);
        }

        // Check for jump input
        // and if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jump();
        }

        // set the animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void jump()
    {
        if (grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, speed);
            anim.SetTrigger("jump");
            // Set grounded to false after jumping
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded by checking if it collides with the ground layer
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
