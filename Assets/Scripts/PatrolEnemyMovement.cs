using UnityEngine;

public class PatrolEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform leftBoundary;
    public Transform rightBoundary;

    private bool movingRight = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            if (transform.position.x >= rightBoundary.position.x)
                movingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            if (transform.position.x <= leftBoundary.position.x)
                movingRight = true;
        }
    }
}
