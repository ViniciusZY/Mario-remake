using UnityEngine;

public class MarioController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movement;

    public float speed = 5f;
    public float jumpForce = 10f;
    public Transform groundChecker;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
