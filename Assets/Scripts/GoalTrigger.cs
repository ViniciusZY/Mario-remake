using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    public GameObject winMenu;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadMainMenu());
        }
    }
    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3f); // Wait for 2 seconds before loading the main menu
        winMenu.SetActive(true);
    }
}
