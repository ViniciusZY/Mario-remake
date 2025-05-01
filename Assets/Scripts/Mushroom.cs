using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
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
}
