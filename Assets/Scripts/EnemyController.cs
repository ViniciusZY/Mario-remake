using UnityEngine;
using Math = System.Math;

public class EnemyController : MonoBehaviour
{
    private Transform transform;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private bool isDead = false;
    private bool isMarioDistant = true;
    private Rigidbody2D rb;
    private float direction = 1;
    public float speed = 1.8f;
    public int scoreValue = 100;

    void Start()
    {
        isMarioDistant = true;
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb.linearVelocity = new Vector2(0, 0);
        animator.SetBool("isDead", false);
    }

    void Update()
    {
        if (isDead) return;
        if (isMarioDistant)
        {
            IsMarioDistant();
            return;
        }

        if (Math.Abs(rb.linearVelocity.x) < 0.1f)
        {
            direction *= -1;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        boxCollider.enabled = false;
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        animator.SetBool("isDead", true);
        GameManager.instance.OnScoreGain(transform.position, scoreValue);
        Destroy(gameObject, 0.5f);
    }

    private void IsMarioDistant()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            float distance = Vector2.Distance(transform.position, camera.transform.position);
            isMarioDistant = Math.Abs(distance) > 20f;
        }
    }
}
