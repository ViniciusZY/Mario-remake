using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform mario;
    public float smoothness = 1f;
    public float followThreshold = 2f;
    public Transform leftLimit;

    void Start()
    {
        UpdateLeftLimitPosition();

    }
    void LateUpdate()
    {
        float targetX = transform.position.x;

        if (mario.position.x > transform.position.x + followThreshold)
        {
            targetX = mario.position.x - followThreshold;
        }

        Vector3 newPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothness);
        UpdateLeftLimitPosition();
    }
    void UpdateLeftLimitPosition()
    {
        if (leftLimit != null)
        {
            float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            leftLimit.position = new Vector3(transform.position.x - cameraHalfWidth, transform.position.y, transform.position.z);
        }
    }
}

