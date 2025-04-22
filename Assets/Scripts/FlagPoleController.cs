using UnityEngine;
using System.Collections;


public class FlagPoleController : MonoBehaviour
{
    public GameObject flag;
    private Transform flagTransform;
    void Start()
    {
        flagTransform = flag.GetComponent<Transform>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveFlagDown());
        }
    }

    private IEnumerator MoveFlagDown()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.flagSound);
        float targetY = flagTransform.position.y - 7.7f;
        float speed = 7f; 

        while (flagTransform.position.y > targetY)
        {
            flagTransform.position = new Vector3(flagTransform.position.x, flagTransform.position.y - speed * Time.deltaTime, flagTransform.position.z);
            yield return null;
        }
    }
}