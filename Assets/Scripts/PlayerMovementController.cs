using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool useAnimation;
    [SerializeField] private bool moveInAir;
    [SerializeField] private bool animateInAir;
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
	public Rigidbody2D _rb 
	{
		get {return rb;}
	}
	public Transform head;
    [Header("Movement")]
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
	
    void Update()
    {
        isGrounded = IsGrounded();
        if (moveInAir)
        { 
            GetMovementInput();
        } else if (isGrounded) GetMovementInput();
        HandleMovement();
        HandleJumping();
    }

    private void GetMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void HandleMovement()
    {
        if (movement.x != 0)
        {
            if (movement.x > 0)
            {
                if (useAnimation && (animateInAir || (!animateInAir && isGrounded)))
                {
                    animator.Play("walk");
                }
                rb.AddForce(Vector2.right * (playerSpeed * Time.deltaTime));
            }
            else
            {
                if (useAnimation && (animateInAir || (!animateInAir && isGrounded)))
                {
                    animator.Play("walkBackwards");
                }
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
