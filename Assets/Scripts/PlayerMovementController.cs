using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] private bool useAnimation;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerSpeed;
    private Vector2 movement;
    [Header("Jumping")]
    [SerializeField] private Vector2 jumpHeight;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayers;
    private bool isGrounded;
    
    void Start()
    {
        //Disable collision for other limbs
        Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = 0; j < colliders.Length; j++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJumping();
    }

    private void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            if (movement.x > 0)
            {
                if (useAnimation) animator.Play("walk");
                rb.AddForce(Vector2.right * (playerSpeed * Time.deltaTime));
            }
            else
            {
                if (useAnimation) animator.Play("walkBackwards");
                rb.AddForce(Vector2.left * (playerSpeed * Time.deltaTime));
            }
        }
        else
        {
            if (useAnimation) animator.Play("idle");
        }
    }

    private void HandleJumping()
    {
        isGrounded = IsGrounded();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
    }
}
