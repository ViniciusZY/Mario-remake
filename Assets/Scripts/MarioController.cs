using UnityEngine;

public class MarioController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movement;

    public float speed = 5f;
    public float jumpForce = 10f;

    private bool isGrounded;
    public LayerMask groundLayer;
    public float raycastDistance = 0.7f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
    }

    void Update()
    {
        isGrounded = IsGrounded();
        movement = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                // Colisão na face direita ou esquerda ou superior
                if (Vector2.Dot(normal, Vector2.left) > 0.9f || Vector2.Dot(normal, Vector2.right) > 0.9f || Vector2.Dot(normal, Vector2.down) > 0.9f)
                {
                    MarioWasHit();
                }
                // Colisão na face inferior
                else if (Vector2.Dot(normal, Vector2.up) > 0.9f)
                {
                    MarioKilledEnemy(collision);
                }
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        return hit.collider != null;
    }

    private void MarioWasHit()
    {
        Debug.Log("Mario morreu!");
    }

    private void MarioKilledEnemy(Collision2D collision)
    {
        Destroy(collision.gameObject);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / 2);
    }
}
