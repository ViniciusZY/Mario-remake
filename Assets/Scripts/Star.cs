using UnityEngine;
using System.Collections;
using Math = System.Math;

public class Star : MonoBehaviour
{
    public float moveSpeed;
    private float movement;
    private bool isSpawning;
    private bool isCheckingDirection = false;


    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;


    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        isSpawning = true;
        movement = -1f;
        StartCoroutine(SpawnAnimation());
    }
    void Update()
    {
        if (!isSpawning)
        {
            if (Math.Abs(rb.linearVelocity.x) < 0.1f && !isCheckingDirection)
            {
                movement *= -1f;
                StartCoroutine(MovementCalculation());
            }
            rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Apply(other.GetComponent<MarioController>());
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;
            // Colisão na face inferior
            if (Vector2.Dot(normal, Vector2.up) > 0.9f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 8f);
                return;
            }
        }
    }


    public void Apply(MarioController mario)
    {
        mario.StartCoroutine(mario.StarMode());
    }

    private IEnumerator SpawnAnimation()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + new Vector3(0, 0.7f, 0);
        float elapsedTime = 0f;
        float duration = 0.5f;

        AudioManager.instance.PlaySFX(AudioManager.instance.powerUpAppearSound);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        boxCollider.enabled = true;
        rb.gravityScale = 1f;
        isSpawning = false;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isSpawning", false);
        }

    }
    private IEnumerator MovementCalculation()
    {
        isCheckingDirection = true;
        yield return new WaitForSeconds(1f);
        isCheckingDirection = false;
    }
}
