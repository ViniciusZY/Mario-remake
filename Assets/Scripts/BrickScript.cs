using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour
{
    private MarioController marioController;
    public GameObject brickParticles;
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    public float bounceHeight = 0.2f;    // Altura máxima do "pulo" do bloco
    public float bounceDuration = 0.2f;  // Duração total da animação (subida e descida)
    private bool isBouncing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            marioController = collision.gameObject.GetComponent<MarioController>();

            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                // Se a colisão ocorrer na face inferior do bloco (Mario bate de baixo)
                if (Vector2.Dot(normal, Vector2.up) > 0.9f)
                {
                    Hit();
                    break;
                }
            }

        }
    }

    void Hit()
    {
        if (!isBouncing)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.bumpSound);
            if (marioController.isBig)
            {
                BreakBlock();
            }
            else
            {
                StartCoroutine(Bounce());
            }
        }
    }

    void BreakBlock()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.brickBreakSound);
        Instantiate(brickParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
