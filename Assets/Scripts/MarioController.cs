using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class MarioController : MonoBehaviour
{
    private Transform transform;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private float movement;
    public float speed = 5f;
    public float jumpForce = 10f;
    public bool isBig = false;

    private bool isGrounded;
    private bool isJumping;
    private bool isDead = false;
    private bool reachedFlag = false;
    public LayerMask groundLayer;
    private float raycastDistance = 0.7f;
    private float raycastWidth;
    private int ignoreGroundedFrames = 0;

    public Transform castleEntryPoint;


    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        raycastWidth = boxCollider.size.x / 2f * transform.localScale.x + 0.014f;
        isGrounded = false;

    }

    void Update()
    {
        if (isDead || reachedFlag) return;

        movement = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
        if (movement != 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(movement), transform.localScale.y, transform.localScale.z);

        if (ignoreGroundedFrames > 0)
        {
            ignoreGroundedFrames--;
            isGrounded = false;
        }
        else
        {
            isGrounded = IsGrounded();
        }
        isJumping = Input.GetButtonDown("Jump") && isGrounded;
        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            ignoreGroundedFrames = 2;
        }

        animator.SetBool("isRunning", movement != 0);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FlagPole") && !reachedFlag)
        {
            reachedFlag = true;
            StartCoroutine(FlagSequence());
        }
    }

    private bool IsGrounded()
    {
        Vector2 center = transform.position;
        Vector2 left = center + Vector2.left * raycastWidth;
        Vector2 right = center + Vector2.right * raycastWidth;

        return
            Physics2D.Raycast(center, Vector2.down, raycastDistance, groundLayer) ||
            Physics2D.Raycast(left, Vector2.down, raycastDistance, groundLayer) ||
            Physics2D.Raycast(right, Vector2.down, raycastDistance, groundLayer);
    }

    private void MarioWasHit()
    {
        Die();
    }

    private void Die()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        boxCollider.enabled = false;

        animator.SetBool("isDead", true);

        rb.linearVelocity = new Vector2(0, 10f);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator FlagSequence()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isClimbing",true);

        yield return new WaitForSeconds(1.2f); // tempo de descida

        animator.SetBool("isClimbing", false);
        Vector3 dest = new Vector3(castleEntryPoint.position.x +2f, castleEntryPoint.position.y-2f, 0);

        while (transform.position.x < castleEntryPoint.position.x+2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 2f * Time.deltaTime);
            yield return null;
        }
    }


    private void MarioKilledEnemy(Collision2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Die();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / 2);
        }
    }
}
