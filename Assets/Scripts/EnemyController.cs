using UnityEngine;
using Math = System.Math;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float direction = -1;
    public float speed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (Math.Abs(rb.linearVelocity.x) < 0.1f)
        {
            direction *= -1;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }


}
