using UnityEngine;

public class FlagPoleController : MonoBehaviour
{
    public GameObject flag;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
