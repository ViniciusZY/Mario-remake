using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform mario;
    public float smoothness = 1f;
    public float followThreshold = 2f; 

    void LateUpdate()
    {
        float targetX = transform.position.x;

        if (mario.position.x > transform.position.x + followThreshold)
        {
            targetX = mario.position.x - followThreshold;
        }

        Vector3 newPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothness);
    }
}
