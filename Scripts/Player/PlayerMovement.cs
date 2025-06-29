using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private void Awake()
    {
        // Refrence the Rigidbody2D and Animator components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");

        // Flip the player sprite based on horizontal input
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3);
        }

        // set the animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocityY);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;
            // Check for jump input
            if (Input.GetKeyDown(KeyCode.Space))
                jump();

        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }
    private bool isGrounded()
    {
        // Check if the player is grounded by casting a ray downwards
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        // The player can attack if they are not moving horizontally, are grounded, and not on a wall
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
