using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void SetScore(int amount)
    {
        scoreText.text = amount.ToString();
    }
}
