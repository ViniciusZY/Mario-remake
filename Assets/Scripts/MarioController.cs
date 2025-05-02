using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class MarioController : MonoBehaviour
{
    public GameObject mario;
    private Transform transform;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private float movement;

    [Header("Movement")]
    public float speed = 5f;

    [Header("Jump")]
    public float initialJumpForce = 10f;
    public float extraJumpForce = 5f;
    public float maxJumpHoldTime = 0.3f;
    private float jumpTimeCounter;

    [Header("Power-Up")]
    public float growDuration = 0.2f;
    public float invincibleDuration = 10f;
    public bool isBig = false;
    private bool isInvincible = false;
    public float growMultiplier;
    private bool isChangingSize = false;
    public bool isStarMode = false;

    [Header("Extra")]
    private bool isGrounded;
    private bool isJumping;
    private bool isDead = false;
    private bool reachedFlag = false;
    public LayerMask groundLayer;
    private float raycastDistance = 0.7f;
    private float raycastWidth;
    private int ignoreGroundedFrames = 0;

    public Transform castleEntryPoint;
    private bool isTouchingLeftLimit = false;




    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        raycastWidth = boxCollider.size.x / 2f * transform.localScale.x + 0.01f;
        raycastDistance = 0.7f;
        isGrounded = false;

    }

    void Update()
    {
        if (isDead || reachedFlag) return;

        movement = Input.GetAxis("Horizontal");
        if (isTouchingLeftLimit && movement < 0 || isChangingSize)
        {
            movement = 0;
        }

        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
        if (movement != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(movement), transform.localScale.y, transform.localScale.z);
        }

        if (!isChangingSize) { CheckJumpInput(); }

        animator.SetBool("isRunning", movement != 0);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);

    }

    private void CheckJumpInput()
    {
        if (ignoreGroundedFrames > 0)
        {
            ignoreGroundedFrames--;
            isGrounded = false;
        }
        else
        {
            isGrounded = IsGrounded();
            if (isGrounded) { isJumping = false; }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetButton("Jump") && isJumping && jumpTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, initialJumpForce + extraJumpForce);
        }

        jumpTimeCounter -= Time.deltaTime;
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
                    if (isStarMode)
                    {
                        MarioKilledEnemy(collision);
                        return;
                    }
                    MarioWasHit();
                    return;
                }
                // Colisão na face inferior
                if (Vector2.Dot(normal, Vector2.up) > 0.9f)
                {
                    MarioKilledEnemy(collision);
                    return;
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
        else if (other.CompareTag("Goal"))
        {
            mario.SetActive(false);
        }
        else if (other.CompareTag("LeftLimit"))
        {
            isTouchingLeftLimit = true;
        }
        else if (other.CompareTag("DownLimit"))
        {
            Die();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftLimit"))
        {
            isTouchingLeftLimit = false;
        }
    }


    private void Jump()
    {
        AudioManager.instance.PlaySFX(isBig ? AudioManager.instance.bigJumpSound : AudioManager.instance.jumpSound);

        isJumping = true;

        jumpTimeCounter = maxJumpHoldTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, initialJumpForce);

        ignoreGroundedFrames = 10;
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
        if (isInvincible) return;
        if (isBig)
        {
            StartCoroutine(PowerDown());
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.LoseLife();
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.marioDiesMusic);
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        boxCollider.enabled = false;

        animator.SetBool("isDead", true);

        rb.linearVelocity = new Vector2(0, 10f);

        yield return new WaitForSeconds(3f);
    }

    private IEnumerator FlagSequence()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isClimbing", true);

        yield return new WaitForSeconds(1.2f);

        AudioManager.instance.PlayMusic(AudioManager.instance.victoryMusic);

        animator.SetBool("isClimbing", false);
        Vector3 dest = new Vector3(castleEntryPoint.position.x + 2f, castleEntryPoint.position.y - 2f, 0);
        animator.SetBool("isRunning", true);
        while (transform.position.x < castleEntryPoint.position.x + 2f)
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
            if (!isStarMode)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.stompSound);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, initialJumpForce * 1.2f);
            }

        }
    }

    public IEnumerator Grow()
    {
        if (isBig)
        {
            yield break;
        }
        GameManager.instance.OnScoreGain(transform.position, 1000);

        isInvincible = true;
        isChangingSize = true;
        isBig = true;
        animator.SetBool("isBig", true);

        AudioManager.instance.PlaySFX(AudioManager.instance.powerUpSound);

        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(start.x, start.y * growMultiplier, start.z);

        float t = 0f;

        while (t < growDuration)
        {
            transform.localScale = Vector3.Lerp(start, end, t / growDuration);
            t += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("Grow");
        transform.localScale = start;
        boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y * growMultiplier);
        raycastDistance = 0.9f;

        isInvincible = false;
        isChangingSize = false;

    }

    public IEnumerator StarMode()
    {
        if (isStarMode) yield break;
        isInvincible = true;
        isStarMode = true;
        speed *= 1.5f;

        AudioManager.instance.PlayMusic(AudioManager.instance.starMusic);

        float blinkDuration = 0.15f;
        float totalDuration = 7.5f;
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkDuration);
            elapsed += blinkDuration * 2;
        }

        spriteRenderer.color = Color.white;
        AudioManager.instance.PlayMusic(AudioManager.instance.backgroundMusic);
        speed /= 1.5f;


        isInvincible = false;
        isStarMode = false;
    }

    public IEnumerator PowerDown()
    {
        isInvincible = true;
        isChangingSize = true;
        isBig = false;
        animator.SetBool("isBig", false);
        AudioManager.instance.PlaySFX(AudioManager.instance.powerDownSound);

        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(start.x, start.y / growMultiplier, start.z);
        float t = 0f;

        while (t < growDuration)
        {
            transform.localScale = Vector3.Lerp(start, end, t / growDuration);
            t += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("PowerDown");
        transform.localScale = start;
        boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y / growMultiplier);
        raycastDistance = 0.7f;

        isInvincible = false;
        isChangingSize = false;
    }
}
