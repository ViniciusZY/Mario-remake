using UnityEngine;
using System.Collections;

public class LuckyBlockScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    public float bounceHeight = 0.2f;
    public float bounceDuration = 0.2f;
    private bool isBouncing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;
            // Se a colisão ocorrer na face inferior do bloco
            if (Vector2.Dot(normal, Vector2.up) > 0.9f)
            {
                Hit();
                break;
            }
        }
    }

    void Hit()
    {
        if (!isBouncing)
        {
            animator.SetBool("wasHit", true);
            StartCoroutine(Bounce());
        }
    }

    IEnumerator Bounce()
    {
        isBouncing = true;
        float halfDuration = bounceDuration / 2f;
        float elapsedTime = 0f;

        // Subida: do original até a altura máxima
        while (elapsedTime < halfDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, originalPosition + Vector2.up * bounceHeight, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition + Vector2.up * bounceHeight;

        // Descida: volta para a posição original
        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            transform.position = Vector2.Lerp(originalPosition + Vector2.up * bounceHeight, originalPosition, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        isBouncing = false;
    }
}
