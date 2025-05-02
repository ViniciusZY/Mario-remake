using UnityEngine;
using System.Collections;
using Math = System.Math;

public class Mushroom : MonoBehaviour
{
    public float moveSpeed = 1.5f;
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


    public void Apply(MarioController mario)
    {
        mario.StartCoroutine(mario.Grow());
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

    }
    private IEnumerator MovementCalculation()
    {
        isCheckingDirection = true;
        yield return new WaitForSeconds(1f);
        isCheckingDirection = false;
    }
}
